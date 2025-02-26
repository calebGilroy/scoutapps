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
.output blahblah.csv
SELECT * FROM raw_match_data;

.quit
```

