del /ah *.suo /s/q
for /r %%a in (obj) do rd "%%a" /s/q
for /r %%a in (bin) do rd "%%a" /s/q
pause
