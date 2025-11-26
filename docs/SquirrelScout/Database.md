# Database

Start the scanner in the Visual Studio Code "Terminal" without compiling:

```powershell
./dk src/SonicScout_Setup/Develop.ml scanner --skip-fetch --quick
```

Opens the database in the Visual Studio Code "Terminal" without compiling:

```powershell
./dk src/SonicScout_Setup/Develop.ml database --skip-fetch --quick
```

To delete the database ... which is needed before a game day, do:

```
del C:\Users\melan\AppData\Local\sonic-scout\sqlite3.db
```

right click if copy pasting doesn't work
After ".quit" a csv file will be created. Right click the file and open with file explorer. 

```sql
.mode line
SELECT * FROM raw_match_data;
SELECT match_number, team_number,endgame_climb, tele_op_coral_l4_score FROM raw_match_data;
SELECT match_number, team_number,endgame_climb, tele_op_coral_l4_score FROM raw_match_data where team_number = 2930; 

.quit
```

```sql
.mode csv
.output blahblah.csv
SELECT * FROM raw_match_data;

.quit
```

