@echo off
echo Compiling...

g++ -o out.exe main.cpp -mwindows -lwinmm -lgdi32 -luser32 -lkernel32 -lgdiplus -lole32 -lwinspool -lcomdlg32


REM Check for compilation errors
if %errorlevel% neq 0 (
    echo Compilation failed! Error code: %errorlevel%.
    echo Please check the code for errors.
    exit /b %errorlevel%
)

echo Compilation successful.
echo Running the executable...

REM Run the compiled program

out.exe

taskkill /f /im out.exe
