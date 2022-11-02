cls
SET p="C:\Users\Startklar\Downloads\curl-7.86.0-win64-mingw\bin"

SET a=http://localhost/double/1024
SET b=http://localhost/halve/512
SET c=http://localhost/cards/0
SET d=http://localhost/cards/1

%p%\curl.exe --http0.9 -X GET %a% --header "Authorization: Bearer tester-mtcgToken"
%p%\curl.exe --http0.9 -X GET %b% --header "Authorization: Bearer tester-mtcgToken"
%p%\curl.exe --http0.9 -X GET %c% --header "Authorization: Bearer tester-mtcgToken"
%p%\curl.exe --http0.9 -X GET %d% --header "Authorization: Bearer tester-mtcgToken"

%p%\curl.exe --http0.9 ^
-X POST http://localhost/cards/test ^
-H "Content-Type: application/json" ^
-H "Authorization: Bearer tester-mtcgToken" ^
-d @data.json
