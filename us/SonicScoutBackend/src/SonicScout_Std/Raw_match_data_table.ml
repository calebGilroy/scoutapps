(*EDITABLE FILE FOR CHANGING APP OVER THE SEASONS*)

module type Fetchable_Data = sig
  val already_contains_record :
    Sqlite3.db ->
    team_number:int ->
    match_number:int ->
    scouter_name:string ->
    bool

  module Fetch : sig
    val latest_match_number : Sqlite3.db -> int option

    val missing_data :
      Sqlite3.db ->
      (int * Intf.Types.robot_position list) list

    val all_match_numbers_in_db : Sqlite3.db -> int list
    val teams_for_match_number : Sqlite3.db -> int -> int list
    (* FIXME: add missing match data function  *)
    (* val average_auto_game_pieces : Sqlite3.db -> float
       val average_auto_cones : Sqlite3.db -> float
       val average_auto_cubes : Sqlite3.db -> float
       val average_auto_cones : Sqlite3.db -> float *)
  end
end

module type Table_type = sig
  include Db_utils.Generic_Table
  include Fetchable_Data
end

type field_type =
  | Integer
  | Bool
  | String
  [@@warning "-unused-type-declaration"]

type field = {
  field_name: string;
  field_type: field_type;
  field_is_primary : bool;
}
let fi field_name = { field_name; field_is_primary=false; field_type = Integer }
let fb field_name = { field_name; field_is_primary=false; field_type = Bool }
let fs field_name = { field_name; field_is_primary=false; field_type = String }
let as_primary field = { field with field_is_primary=true }

let fields = [
  fs "Name";
  fi "Team";
  fi "Match";
  fs "Alliance";
  fs "SPos";
  fs "AMove";
  fi "AFS";
  fi "AFM";
  fs "ATC";
  fb "ABump";
  fb "ATrench";
  fb "AIDep";
  fb "AIOut";
  fb "AINZ";
  fi "TFS";
  fi "TFM";
  fb "TBump";
  fb "TTrench";
  fb "TIDep";
  fb "TIOut";
  fb "TINZ";
  fs "TC";
  fs "TB";
]

let getfield name =
  List.find_opt
    (fun { field_name; _} -> String.equal field_name name)
    fields

let primaryfields = List.filter_map
  (fun {field_is_primary; field_name; field_type=_} ->
    if field_is_primary then Some field_name else None) fields

(*Code which can be edited for each specific season*)
module Table : Table_type = struct
  let table_name = "raw_match_data"

  (*Code which can be modified based on what data points are wanted for
    that specific robotics season*)
  type colums =
    (* [not game specific] generic info *)
    | Team_number
    | Match_Number
    | Scouter_Name
    | Alliance_Color
    (* [Game specific] auto*)
    | Starting_Position
    | Auto_move
    | Auto_fuel_score
    | Auto_fuel_miss
    | Auto_tower_climb
    | Auto_bump
    | Auto_trench
    | Auto_intake_depot
    | Auto_intake_outpost
    | Auto_intake_neutral_zone

    (* [Game specific] tele data points *)
    | Tele_op_fuel_score
    | Tele_op_fuel_miss
    | Tele_op_bump
    | Tele_op_trench
    | Tele_op_intake_depot
    | Tele_op_intake_outpost
    | Tele_op_intake_neutral_zone
    | Tower_climb
    | Breakdown

  (* This is how the column will be seen in the SQL database or the
    .csv file upon exportation*)
  (*The # of column_name should match the # of columns defined in above function*)
  let colum_name = function
    | Team_number -> "team_number"
    | Match_Number -> "match_number"
    | Scouter_Name -> "scouter_name"
    | Alliance_Color -> "alliance_color"
    (*  *)
    | Starting_Position -> "starting_position"
    | Auto_move -> "auto_move"
    | Auto_fuel_score -> "auto_fuel_score"
    | Auto_fuel_miss -> "auto_fuel_miss"
    | Auto_tower_climb -> "auto_tower_climb"
    | Auto_bump -> "auto_bump"
    | Auto_trench -> "auto_trench"
    | Auto_intake_depot -> "auto_intake_depot"
    | Auto_intake_outpost -> "auto_intake_outpost"
    | Auto_intake_neutral_zone -> "auto_intake_neutral_zone"
    (*  *)
    | Tele_op_fuel_score -> "tele_op_fuel_score"
    | Tele_op_fuel_miss -> "tele_op_fuel_miss"
    | Tele_op_bump -> "tele_op_bump"
    | Tele_op_trench -> "tele_op_trench"
    | Tele_op_intake_depot -> "tele_op_intake_depot"
    | Tele_op_intake_outpost -> "tele_op_intake_outpost"
    | Tele_op_intake_neutral_zone -> "tele_op_intake_neutral_zone"
    | Tower_climb -> "tower_climb"
    | Breakdown -> "breakdown"

  (* This defines what kind of data type each column will store. There are only two options: TEXT or INT*)
  (* # of colum_datatype should be matching # of columns defined in above functions*)
  let colum_datatype = function
    | Team_number -> "INT"
    | Match_Number -> "INT"
    | Scouter_Name -> "TEXT"
    | Alliance_Color -> "TEXT"
    (*  *)
    | Starting_Position -> "TEXT"
    | Auto_move -> "BOOLEAN"
    | Auto_fuel_score -> "INT"
    | Auto_fuel_miss -> "INT"
    | Auto_tower_climb -> "TEXT"
    | Auto_bump -> "BOOLEAN"
    | Auto_trench -> "BOOLEAN"
    | Auto_intake_depot -> "BOOLEAN"
    | Auto_intake_outpost -> "BOOLEAN"
    | Auto_intake_neutral_zone -> "BOOLEAN"
    (*  *)
    | Tele_op_fuel_score -> "INT"
    | Tele_op_fuel_miss -> "INT"
    | Tele_op_bump -> "BOOLEAN"
    | Tele_op_trench -> "BOOLEAN"
    | Tele_op_intake_depot -> "BOOLEAN"
    | Tele_op_intake_outpost -> "BOOLEAN"
    | Tele_op_intake_neutral_zone -> "BOOLEAN"
    | Tower_climb -> "TEXT"
    | Breakdown -> "TEXT"

  (*This defines how the order of the columns seen in databse will be written and captured from QR code*)
  let colums_in_order =
    [
      Team_number;
      Match_Number;
      Scouter_Name;
      Alliance_Color;
      (*Alliance;*)
      (* Auto *)
      Starting_Position;
      Auto_move;
      Auto_fuel_score;
      Auto_fuel_miss;
      Auto_bump;
      Auto_trench;
      Auto_intake_depot;
      Auto_intake_outpost;
      Auto_intake_neutral_zone;
      Auto_tower_climb;
      (* Teleop *)
      Tele_op_fuel_score;
      Tele_op_fuel_miss;
      Tele_op_bump;
      Tele_op_trench;
      Tele_op_intake_depot;
      Tele_op_intake_outpost;
      Tele_op_intake_neutral_zone;
      Tower_climb;
      Breakdown;
    ]

  (*Set's primary keys for identification of data. Do not change under normal circumstanes.*)
  let primary_keys = [ Team_number; Match_Number; Scouter_Name ]

  let create_table db =
    let columns = Buffer.create 1024 in
    List.iteri
      (fun idx { field_name; field_type=_; field_is_primary=_ } ->
            if (idx > 0) then (
              Buffer.add_string columns " ,";
            );
            Printf.bprintf columns "%s" field_name)
      fields;
    let createtable = Printf.sprintf "CREATE TABLE IF NOT EXISTS %s (%s, PRIMARY KEY (%s))"
      table_name
      (Buffer.contents columns)
      (String.concat "," primaryfields)
    in
    match Sqlite3.exec db createtable with
    | Sqlite3.Rc.OK ->
        print_endline "create table successful";
        Db_utils.Successful
    | r ->
        Db_utils.formatted_error_message db r
          "failed to exec raw_match_data create sql";
        Db_utils.Failed

  let drop_table _db = Db_utils.Failed

  let already_contains_record db ~team_number ~match_number ~scouter_name =
    let to_select = colum_name Team_number in
    let where =
      [
        (colum_name Team_number, Db_utils.Select.Int team_number);
        (colum_name Match_Number, Db_utils.Select.Int match_number);
        (colum_name Scouter_Name, Db_utils.Select.String scouter_name);
      ]
    in

    let result =
      Db_utils.Select.select_ints_where db ~table_name ~to_select ~where
    in

    match result with _ :: [] -> true | _ -> false

  (** The QR code format is carriage-return, line-feed separated
      name value parameters.

      An example code is the following, broken onto many lines
      for readability:

      {v
        Name-\r\nSPos-Left\r\nTeam-0\r\n
        Match-0\r\nALeave-True\r\nAL4-0\r\n
        AL3-0\r\nAL2-0\r\nAL1-0\r\nAM4-0\r\n
        AM3-0\r\nAM2-0\r\nAM1-0\r\n
        GPickup-False\r\nTL4-0\r\nTL3-0\r\n
        TL2-0\r\nTL1-0\r\nTM4-0\r\nTM3-0\r\n
        TM2-0\r\nTM1-0\r\nAPS-0\r\nAPM-0\r\n
        ANS-0\r\nANM-0\r\nTPS-0\r\nTPM-0\r\n
        TNS-0\r\nTNM-0\r\nTBK-None\r\nCLB-Success
      v}

      For example, the name ["GPickup"] has the value ["False"].
    *)
  let insert_record db qr_string =
    (* NAME1-VALUE1\r\nNAME2-VALUE2\r\n...
       ==>
       NAME1-VALUE1\r
       NAME2-VALUE2\r
       ... *)
    let namevalues = String.split_on_char '\n' qr_string in
    (* NAME1-VALUE1\r
       NAME2-VALUE2\r
       ==>
       NAME1-VALUE1
       NAME2-VALUE2
       ... *)
    let namevalues = List.map String.trim namevalues in
    (* NAME1-VALUE1
       NAME2-VALUE2
       ==>
       (NAME1,VALUE1)
       (NAME2,VALUE2)
       ... *)
    let namevalues = List.filter_map (fun s ->
      let splitted = String.split_on_char '-' s in
      match splitted with
      | [name; value] ->
        (* Printf.eprintf "(name, value) = (%s, %s)\n%!" name value; *)
        Some (name, value)
      | _ -> None
      ) namevalues in

    (* This uses string manipulation to make a SQL command ...
       it follows what the original authors did in 2023.
       If you were making an application for a customer, this would
       be a security vulnerability because a malicious person
       could make a QR code that ran arbitrary commands on your
       computer. Not good! It is an attack called
       "SQL injection attack". *)
    let column_names = Buffer.create 1024 in
    let field_values = Buffer.create 1024 in
    List.iteri
      (fun idx (name, value) ->
        match getfield name with
        | None ->
            Printf.eprintf "WARNING! The QR code contained `%s` but the QR scanning code in %s.ml did not add a field for it.\n" name __MODULE__
        | Some { field_name; field_type=_; field_is_primary=_ } ->
            if (idx > 0) then (
              Buffer.add_string column_names " ,";
              Buffer.add_string field_values " ,";
            );
            Buffer.add_string column_names field_name;
            Buffer.add_string field_values ("\"" ^ value ^ "\""))
      namevalues;

    let sql = Printf.sprintf "INSERT INTO %s(%s) VALUES(%s)"
          table_name
          (Buffer.contents column_names)
          (Buffer.contents field_values)
    in
    Logs.info (fun l -> l "raw_match_table sql: %s" sql);

    match Sqlite3.exec db sql with
    | Sqlite3.Rc.OK ->
        print_endline "exec successful";
        Db_utils.Successful
    | r ->
        Db_utils.formatted_error_message db r
          "failed to exec raw_match_data insert sql";
        Db_utils.Failed


  (** This is for inserting a record using the Capnp cross-platform
      format which was used for communicating in binary inside
      Android between Java (most Android code is Java) and C/OCaml
      (the QR scanner is mostly C).

      We used this from 2023-2025 but not anymore. *)
  let insert_record_capnp db capnp_string =
    let module ProjectSchema = Schema.Make (DkSDKFFI_OCaml0.ComMessageC) in
    let match_data =
      (* Get a stream of the bytes to deserialize *)
      let host_segment_allocator = DkSDKFFI_OCaml0.HostStorageOptions.C_Options.host_segment_allocator in
      let stream =
        DkSDKFFI_OCaml0.ComCodecs.FramedStreamC.of_string
          ~host_segment_allocator
          ~compression:`None capnp_string
      in
      (* Attempt to get the next frame from the stream in the form of a Message *)
      match DkSDKFFI_OCaml0.ComCodecs.FramedStreamC.get_next_frame stream
      with
      | Result.Ok message -> ProjectSchema.Reader.RawMatchData.of_message message
      | Result.Error _ -> failwith (Printf.sprintf "could not decode capnp data. instead got: %s" capnp_string)
    in

    (*All these following functions give function on how the Enum data created in schema.capnp will be read and written*)
    let position_to_string : ProjectSchema.Reader.SPosition.t -> string = function
      | AmpSide -> "Left"
      | Center -> "CENTER"
      | SourceSide -> "Right"
      | Undefined _ -> "UNDEFINED"
    in

    let breakdown_to_string : ProjectSchema.Reader.TBreakdown2025.t -> string = function
      | None -> "NONE"
      | Tipped -> "TIPPED"
      | MechanicalFailure -> "MECHANICAL_FAILURE"
      | Incapacitated -> "INCAPACITATED"
      | Undefined _ -> "NONE"
      | GamePieceStuck -> "GAMEPIECESTUCK"
      | StuckOnAlgae -> "StuckOnAlgae"
      | CoralStuck -> "CoralStuck"
    in

    let teleopClimb_to_string : ProjectSchema.Reader.EClimb2025.t -> string = function
      | DeepCage -> "Deep_Cage"
      | Failed -> "FAILED"
      | DidNotAttempt -> "DID_NOT_ATTEMPT"
      | ShallowCage -> "Shallow_Cage"
      | Parked -> "PARKED"
      | Undefined _ -> "UNDEFINED"
      | Success -> "SUCCESS"
    in

    (* let alliance_to_string : ProjectSchema.Reader.RobotPosition.t -> string = function
      | Red1 -> "RED1"
      | Red2 -> "RED2"
      | Red3 -> "RED3"
      | Blue1 -> "BLUE1"
      | Blue2 -> "BLUE2"
      | Blue3 -> "BLUE3"
      | Undefined _ -> "UNDEFINED"
    in *)

    let string_to_cmd_line_form s = "\"" ^ s ^ "\"" in

    let bool_to_string_as_num bool =
      match bool with true -> "1" | false -> "0"
    in

    let open ProjectSchema.Reader.RawMatchData in
    let team_number = match_data |> team_number_get in
    let match_number = match_data |> match_number_get in
    let scouter_name = match_data |> scouter_name_get in

    let record_already_exists =
      already_contains_record db ~team_number ~match_number ~scouter_name
    in
    (*Code if you wanted to check something quick from the outputs when scanning QR codes*)
    Format.eprintf "auto_speaker_miss_get = %d@." (auto_speaker_miss_get match_data);

    if record_already_exists then Db_utils.Successful
    else
      (* RELEASE_BLOCKER: jonahbeckford@

         This is not how to insert data into a database.
         It is INCREDIBLY unsafe, although the OCaml library
         does not give you any examples of how to do it safely
         with prepared statements. All someone would need to
         do is make a special QR code and they could hack your phone. *)
      let values =
        Printf.sprintf
        (*Number of %s should match number of columns*)
        "
        %s, %s, %s, %s,
        %s, %s, %s, %s,
        %s, %s, %s, %s,
        %s, %s, %s, %s,
        %s, %s, %s, %s,
        %s, %s, %s, %s,
        %s, %s, %s, %s,
        %s, %s, %s, %s,
        %s
        "
        (match_data |> team_number_get |> string_of_int)
        (match_data |> team_name_get |> string_to_cmd_line_form)
        (match_data |> match_number_get |> string_of_int)
        (match_data |> scouter_name_get |> string_to_cmd_line_form)
        (*  *)
        (match_data |> starting_position_get |> position_to_string|> string_to_cmd_line_form)
        (match_data |> auto_coral_l1_score_get |> string_of_int)
        (match_data |> auto_coral_l1_miss_get |> string_of_int)
        (match_data |> auto_coral_l2_score_get |> string_of_int)
        (match_data |> auto_coral_l2_miss_get |> string_of_int)
        (match_data |> auto_coral_l3_score_get |> string_of_int)
        (match_data |> auto_coral_l3_miss_get |> string_of_int)
        (match_data |> auto_coral_l4_score_get |> string_of_int)
        (match_data |> auto_coral_l4_miss_get |> string_of_int)
        (match_data |> auto_processor_score_get |> string_of_int)
        (match_data |> auto_processor_miss_get |> string_of_int)
        (match_data |> auto_net_score_get |> string_of_int)
        (match_data |> auto_net_miss_get |> string_of_int)
        (match_data |> preplaced_coral_get |> bool_to_string_as_num)
        (match_data |> auto_leave_get |> bool_to_string_as_num)

        (*  *)
        (match_data |> tele_op_coral_l1_score_get |> string_of_int)
        (match_data |> tele_op_coral_l1_miss_get |> string_of_int)
        (match_data |> tele_op_coral_l2_score_get |> string_of_int)
        (match_data |> tele_op_coral_l2_miss_get |> string_of_int)
        (match_data |> tele_op_coral_l3_score_get |> string_of_int)
        (match_data |> tele_op_coral_l3_miss_get |> string_of_int)
        (match_data |> tele_op_coral_l4_score_get |> string_of_int)
        (match_data |> tele_op_coral_l4_miss_get |> string_of_int)
        (match_data |> tele_op_processor_score_get |> string_of_int)
        (match_data |> tele_op_processor_miss_get |> string_of_int)
        (match_data |> tele_op_net_score_get |> string_of_int)
        (match_data |> tele_op_net_miss_get |> string_of_int)
        (match_data |> tele_op_breakdown_get |> breakdown_to_string|> string_to_cmd_line_form)
        (match_data |> tele_op_climb_get |> teleopClimb_to_string|> string_to_cmd_line_form)
      in

      let sql = "INSERT INTO " ^ table_name ^ " VALUES(" ^ values ^ ")" in

      Logs.debug (fun l -> l "raw_match_table sql: %s" sql);

      match Sqlite3.exec db sql with
      | Sqlite3.Rc.OK ->
          print_endline "exec successful";
          Db_utils.Successful
      | r ->
          Db_utils.formatted_error_message db r
            "failed to exec raw_match_data insert sql";
          Db_utils.Failed
      [@@warning "-unused-value-declaration"]

  module Fetch = struct
    let latest_match_number db =
      let to_select = colum_name Match_Number in

      let where = [] in

      let order_by = [ (colum_name Match_Number, Db_utils.Select.DESC) ] in

      let result =
        Db_utils.Select.select_ints_where db ~table_name ~to_select ~where
          ~order_by
      in

      match result with [] -> None | x :: _ -> Some x

    let all_match_numbers_in_db db =
      let to_select = colum_name Match_Number in

      Db_utils.Select.select_ints_where db ~table_name ~to_select ~where:[]

    let teams_for_match_number db match_num =
      let to_select = colum_name Team_number in
      let where =
        [ (colum_name Match_Number, Db_utils.Select.Int match_num) ]
      in

      Db_utils.Select.select_ints_where db ~table_name ~to_select ~where

    let missing_data db =
      (* let scheduled_matches =
        Match_schedule_table.Table.Fetch.get_all_match_numbers db
      in *)

      (* let all_matches_in_db = all_match_numbers_in_db db in *)

      let latest_match = latest_match_number db in

      match latest_match with
      | None -> []
      | Some l_match ->
          let num_entries_for_match_num match_num =
            let to_select = colum_name Match_Number in
            let where =
              [ (colum_name Match_Number, Db_utils.Select.Int match_num) ]
            in

            List.length
              (Db_utils.Select.select_ints_where db ~table_name ~to_select
                 ~where)
          in

          let rec build_missing_lst lst current_match =
            match current_match > l_match with
            | true -> lst
            | false ->
                let num_entries = num_entries_for_match_num current_match in
                let new_lst =
                  if num_entries < 6 then current_match :: lst else lst
                in

                build_missing_lst new_lst (current_match + 1)
          in

          let missing_data_matches = build_missing_lst [] 1 in

          let teams_missing_per_match_list =
            let rec build matches_missing_data lst =
              match matches_missing_data with
              | [] -> lst
              | match_n :: l ->
                  let teams_scheduled =
                    Match_schedule_table.Table.Fetch.get_all_teams_for_match db
                      match_n
                  in
                  let teams_actually_in_db =
                    teams_for_match_number db match_n
                  in

                  let teams_missing_for_this_match =
                    let rec fliter all_teams missing =
                      match all_teams with
                      | x :: l ->
                          if List.exists (fun a -> a == x) teams_actually_in_db
                          then fliter l missing
                          else fliter l (x :: missing)
                      | [] -> missing
                    in

                    fliter teams_scheduled []
                  in

                  build l ((match_n, teams_missing_for_this_match) :: lst)
            in

            build missing_data_matches []
          in

          let positions_missing_per_match =
            let rec build teams_list pose_list =
              match teams_list with
              | (match_n, lst) :: l ->
                  let rec build_pos_list team_nums pos_lst =
                    match team_nums with
                    | [] -> pos_lst
                    | x :: l ->
                        let p =
                          Match_schedule_table.Table.Fetch
                          .get_position_for_team_and_match db x match_n
                        in
                        build_pos_list l (p :: pos_lst)
                  in

                  let poses = build_pos_list lst [] in

                  build l ((match_n, poses) :: pose_list)
              | [] -> pose_list
            in

            build teams_missing_per_match_list []
          in

          let de_optioned_positions =
            let rec build pose_lst no_opt_lst =
              match pose_lst with
              | [] -> no_opt_lst
              | (match_n, opt_poses) :: l ->
                  let rec de_opt_lst opt_poses de_opted =
                    match opt_poses with
                    | [] -> de_opted
                    | Some x :: l ->
                        let d = x :: de_opted in
                        de_opt_lst l d
                    | None :: l -> de_opt_lst l de_opted
                  in

                  let non_optioned = de_opt_lst opt_poses [] in

                  build l ((match_n, non_optioned) :: no_opt_lst)
            in

            build positions_missing_per_match []
          in

          de_optioned_positions
  end
end
