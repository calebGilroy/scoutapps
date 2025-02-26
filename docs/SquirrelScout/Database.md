# Database

Start the scanner in the Visual Studio Code "Terminal" without compiling:

```powershell
./dk src/SonicScout_Setup/Develop.ml scanner --skip-fetch --quick
```

Opens the database in the Visual Studio Code "Terminal" without compiling:

```powershell
./dk src/SonicScout_Setup/Develop.ml database --skip-fetch --quick
```

```sql
.mode line
SELECT * FROM raw_match_data;

.quit
```

