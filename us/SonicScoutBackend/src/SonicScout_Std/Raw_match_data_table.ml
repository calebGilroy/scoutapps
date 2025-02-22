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

(*Code which can be edited for each specific season*)
module Table : Table_type = struct
  let table_name = "raw_match_data"

  (*Code which can be modified based on what data points are wanted for
    that specific robotics season*)
  type colums =
    (* [not game specific] generic info *)
    | Team_number
    | Team_name
    | Match_Number
    | Scouter_Name
    (* [Game specific] auto*)
    | Starting_Position
    | Auto_coral_l1_score
    | Auto_coral_l1_miss
    | Auto_coral_l2_score
    | Auto_coral_l2_miss
    | Auto_coral_l3_score
    | Auto_coral_l3_miss
    | Auto_coral_l4_score
    | Auto_coral_l4_miss
    | Auto_processor_score
    | Auto_processor_miss
    | Auto_net_score
    | Auto_net_miss
    | Preplaced_coral

    (* [Game specific] tele data points *)
    | Tele_op_coral_l1_score
    | Tele_op_coral_l1_miss
    | Tele_op_coral_l2_score
    | Tele_op_coral_l2_miss
    | Tele_op_coral_l3_score
    | Tele_op_coral_l3_miss
    | Tele_op_coral_l4_score
    | Tele_op_coral_l4_miss
    | Tele_op_processor_score
    | Tele_op_processor_miss
    | Tele_op_net_score
    | Tele_op_net_miss
    | Tele_op_breakdown
    | Endgame_climb

  (* This is how the column will be seen in the SQL database or the
    .csv file upon exportation*)
  (*The # of column_name should match the # of columns defined in above function*)
  let colum_name = function
    | Team_number -> "team_number"
    | Team_name -> "team_name"
    | Match_Number -> "match_number"
    | Scouter_Name -> "scouter_name"
    (*  *)
    | Starting_Position -> "starting_position"
    | Auto_coral_l1_score -> "auto_coral_l1_score"
    | Auto_coral_l1_miss -> "auto_coral_l1_miss"
    | Auto_coral_l2_score -> "auto_coral_l2_score"
    | Auto_coral_l2_miss -> "auto_coral_l2_miss"
    | Auto_coral_l3_score -> "auto_coral_l3_score"
    | Auto_coral_l3_miss -> "auto_coral_l3_miss"
    | Auto_coral_l4_score -> "auto_coral_l4_score"
    | Auto_coral_l4_miss -> "auto_coral_l4_miss"
    | Auto_processor_score -> "auto_processor_score"
    | Auto_processor_miss -> "auto_processor_miss"
    | Auto_net_score -> "auto_net_score"
    | Auto_net_miss -> "auto_net_miss"
    | Preplaced_coral -> "preplaced_coral"

    (*  *)
    | Tele_op_coral_l1_score -> "tele_op_coral_l1_score"
    | Tele_op_coral_l1_miss -> "tele_op_coral_l1_miss"
    | Tele_op_coral_l2_score -> "tele_op_coral_l2_score"
    | Tele_op_coral_l2_miss -> "tele_op_coral_l2_miss"
    | Tele_op_coral_l3_score -> "tele_op_coral_l3_score"
    | Tele_op_coral_l3_miss -> "tele_op_coral_l3_miss"
    | Tele_op_coral_l4_score -> "tele_op_coral_l4_score"
    | Tele_op_coral_l4_miss -> "tele_op_coral_l4_miss"
    | Tele_op_processor_score -> "tele_op_processor_score"
    | Tele_op_processor_miss -> "tele_op_processor_miss"
    | Tele_op_net_score -> "tele_op_net_score"
    | Tele_op_net_miss -> "tele_op_net_miss"
    | Tele_op_breakdown -> "tele_op_breakdown"
    | Endgame_climb -> "endgame_climb"

  (* This defines what kind of data type each column will store. There are only two options: TEXT or INT*)
  (* # of colum_datatype should be matching # of columns defined in above functions*)
  let colum_datatype = function
    | Team_number -> "INT"
    | Team_name -> "TEXT"
    | Match_Number -> "INT"
    | Scouter_Name -> "TEXT"
    (*  *)
    | Starting_Position -> "TEXT"
    | Auto_coral_l1_score -> "INT"
    | Auto_coral_l1_miss -> "INT"
    | Auto_coral_l2_score -> "INT"
    | Auto_coral_l2_miss -> "INT"
    | Auto_coral_l3_score -> "INT"
    | Auto_coral_l3_miss -> "INT"
    | Auto_coral_l4_score -> "INT"
    | Auto_coral_l4_miss -> "INT"
    | Auto_processor_score -> "INT"
    | Auto_processor_miss -> "INT"
    | Auto_net_score -> "INT"
    | Auto_net_miss -> "INT"
    | Preplaced_coral -> "TEXT"

    (*  *)
    | Tele_op_coral_l1_score -> "INT"
    | Tele_op_coral_l1_miss -> "INT"
    | Tele_op_coral_l2_score -> "INT"
    | Tele_op_coral_l2_miss -> "INT"
    | Tele_op_coral_l3_score -> "INT"
    | Tele_op_coral_l3_miss -> "INT"
    | Tele_op_coral_l4_score -> "INT"
    | Tele_op_coral_l4_miss -> "INT"
    | Tele_op_processor_score -> "INT"
    | Tele_op_processor_miss -> "INT"
    | Tele_op_net_score -> "INT"
    | Tele_op_net_miss -> "INT"
    | Tele_op_breakdown -> "TEXT"
    | Endgame_climb -> "TEXT"

  (*This defines how the order of the columns seen in databse will be written and captured from QR code*)
  let colums_in_order =
    [
      Team_number;
      Team_name;
      Match_Number;
      Scouter_Name;
      (*Alliance;*)
      (* Auto *)
      Starting_Position;
      Auto_coral_l1_score;
      Auto_coral_l1_miss;
      Auto_coral_l2_score;
      Auto_coral_l2_miss;
      Auto_coral_l3_score;
      Auto_coral_l3_miss;
      Auto_coral_l4_score;
      Auto_coral_l4_miss;
      Auto_processor_score;
      Auto_processor_miss;
      Auto_net_score;
      Auto_net_miss;
      Preplaced_coral;

      (* Teleop *)
      Tele_op_coral_l1_score;
      Tele_op_coral_l1_miss;
      Tele_op_coral_l2_score;
      Tele_op_coral_l2_miss;
      Tele_op_coral_l3_score;
      Tele_op_coral_l3_miss;
      Tele_op_coral_l4_score;
      Tele_op_coral_l4_miss;
      Tele_op_processor_score;
      Tele_op_processor_miss;
      Tele_op_net_score;
      Tele_op_net_miss;
      Tele_op_breakdown;
      Endgame_climb;
    ]

  (*Set's primary keys for identification of data. Do not change under normal circumstanes.*)
  let primary_keys = [ Team_number; Match_Number; Scouter_Name ]

  let create_table db =
    Db_utils.create_table db ~table_name ~colums:colums_in_order ~primary_keys
      ~to_name:colum_name ~to_datatype:colum_datatype

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

  let insert_record db capnp_string =
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
      | Result.Error _ -> failwith "could not decode capnp data"
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
    in

    let teleopClimb_to_string : ProjectSchema.Reader.EClimb.t -> string = function
      | Harmony -> "Deep_Cage"
      | Failed -> "FAILED"
      | DidNotAttempt -> "DID_NOT_ATTEMPT"
      (* | ShallowCage -> "Shallow_Cage" *)
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
        %s, %s, %s, %s
        "
        (* "%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, \n\
        \         %s, %s, %s, %s, %s, %s, %s, %s, %s, %s" *)
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
        (match_data |> endgame_climb_get |> teleopClimb_to_string|> string_to_cmd_line_form)
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
