

REM --------------------------------------------------
REM Monster Trading Cards Game
REM --------------------------------------------------
title Monster Trading Cards Game
echo CURL Testing for Monster Trading Cards Game
pause

REM --------------------------------------------------
echo 1) Create Users (Registration)
REM Create User
curl --http0.9 -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
pause
curl --http0.9 -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
pause
curl --http0.9 -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
pause

echo should fail:
curl --http0.9 -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
pause
curl --http0.9 -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
pause 
pause

REM --------------------------------------------------
echo 2) Login Users
curl --http0.9 -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
pause
curl --http0.9 -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
pause
curl --http0.9 -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
pause

echo should fail:
curl --http0.9 -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
pause
pause

REM --------------------------------------------------
echo 3) create packages (done by "admin")
curl --http0.9 -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"id\":\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"params\": [\"Goblin\", \"Monster\", \"Water\", 10], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}] }, {\"id\":\"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"params\": [ \"Dragon\", \"Monster\", \"Normal\", 50]}, {\"id\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"params\":[\"Spell\", \"Spell\", \"Water\", 20]}, {\"id\":\"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"params\":[\"Ork\", \"Monster\",\"Normal\", 45]}, {\"id\":\"dfdd758f-649c-40f9-ba3a-8657f4b3439f\", \"params\":[\"Spell\", \"Spell\", \"Fire\"  , 25]}]"
curl --http0.9 -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"id\":\"644808c2-f87a-4600-b313-122b02322fd5\", \"params\": [\"Goblin\", \"Monster\", \"Water\",  9], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}] }, {\"id\":\"4a2757d6-b1c3-47ac-b9a3-91deab093531\", \"params\": [ \"Dragon\", \"Monster\", \"Normal\", 55]}, {\"id\":\"91a6471b-1426-43f6-ad65-6fc473e16f9f\", \"params\":[\"Spell\", \"Spell\", \"Water\", 21]}, {\"id\":\"4ec8b269-0dfa-4f97-809a-2c63fe2a0025\", \"params\":[\"Ork\", \"Monster\",\"Normal\", 55]}, {\"id\":\"f8043c23-1534-4487-b66b-238e0c3c39b5\", \"params\":[\"Spell\", \"Spell\", \"Water\" , 23]}]"
curl --http0.9 -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"id\":\"b017ee50-1c14-44e2-bfd6-2c0c5653a37c\", \"params\": [\"Goblin\", \"Monster\", \"Water\", 11], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}] }, {\"id\":\"d04b736a-e874-4137-b191-638e0ff3b4e7\", \"params\": [ \"Dragon\", \"Monster\", \"Normal\", 70]}, {\"id\":\"88221cfe-1f84-41b9-8152-8e36c6a354de\", \"params\":[\"Spell\", \"Spell\", \"Water\", 22]}, {\"id\":\"1d3f175b-c067-4359-989d-96562bfa382c\", \"params\":[\"Ork\", \"Monster\",\"Normal\", 40]}, {\"id\":\"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\", \"params\":[\"Spell\", \"Spell\", \"Normal\", 28]}]"
curl --http0.9 -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"id\":\"ed1dc1bc-f0aa-4a0c-8d43-1402189b33c8\", \"params\": [\"Goblin\", \"Monster\", \"Water\", 10], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}] }, {\"id\":\"65ff5f23-1e70-4b79-b3bd-f6eb679dd3b5\", \"params\": [ \"Dragon\", \"Monster\", \"Normal\", 50]}, {\"id\":\"55ef46c4-016c-4168-bc43-6b9b1e86414f\", \"params\":[\"Spell\", \"Spell\", \"Water\", 20]}, {\"id\":\"f3fad0f2-a1af-45df-b80d-2e48825773d9\", \"params\":[\"Ork\", \"Monster\",\"Normal\", 45]}, {\"id\":\"8c20639d-6400-4534-bd0f-ae563f11f57a\", \"params\":[\"Spell\", \"Spell\", \"Water\" , 25]}]"
curl --http0.9 -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"id\":\"d7d0cb94-2cbf-4f97-8ccf-9933dc5354b8\", \"params\": [\"Goblin\", \"Monster\", \"Water\",  9], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}] }, {\"id\":\"44c82fbc-ef6d-44ab-8c7a-9fb19a0e7c6e\", \"params\": [ \"Dragon\", \"Monster\", \"Normal\", 55]}, {\"id\":\"2c98cd06-518b-464c-b911-8d787216cddd\", \"params\":[\"Spell\", \"Spell\", \"Water\", 21]}, {\"id\":\"951e886a-0fbf-425d-8df5-af2ee4830d85\", \"params\":[\"Ork\", \"Monster\",\"Normal\", 55]}, {\"id\":\"dcd93250-25a7-4dca-85da-cad2789f7198\", \"params\":[\"Spell\", \"Spell\", \"Fire\"  , 23]}]"																																																																																	 				    
curl --http0.9 -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"id\":\"b2237eca-0271-43bd-87f6-b22f70d42ca4\", \"params\": [\"Goblin\", \"Monster\", \"Water\", 11], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}] }, {\"id\":\"9e8238a4-8a7a-487f-9f7d-a8c97899eb48\", \"params\": [ \"Dragon\", \"Monster\", \"Normal\", 70]}, {\"id\":\"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"params\":[\"Spell\", \"Spell\", \"Water\", 22]}, {\"id\":\"fc305a7a-36f7-4d30-ad27-462ca0445649\", \"params\":[\"Ork\", \"Monster\",\"Normal\", 40]}, {\"id\":\"84d276ee-21ec-4171-a509-c1b88162831c\", \"params\":[\"Spell\", \"Spell\", \"Normal\", 28]}]"
pause

REM --------------------------------------------------
echo 4) acquire packages kienboec
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
pause
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
pause
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
pause
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
pause
echo should fail (no money):
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
pause

echo Adding virtual coins
curl --http0.9 -X PUT http://localhost:10001/transactions/coins/kienboec --header "Content-Type: text/plain"  --header "Authorization: Bearer admin-mtcgToken" -d "10"
pause
curl --http0.9 -X POST http://localhost:10001/transactions/coins/kienboec --header "Content-Type: text/plain"  --header "Authorization: Bearer kienboec-mtcgToken" -d "10"
pause

REM --------------------------------------------------
echo 5) acquire packages altenhof
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
pause
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
pause
echo should fail (no package):
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
pause
pause

REM --------------------------------------------------
echo 6) add new packages
curl --http0.9 -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"id\":\"67f9048f-99b8-4ae4-b866-d8008d00c53d\", \"params\":[\"Goblin\", \"Monster\",\"Water\", 10], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}]}, {\"id\":\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"params\":[\"Spell\", \"Spell\",  \"Normal\", 50]}, {\"id\":\"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"params\":[\"Knight\", \"Monster\",\"Normal\", 20],\"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Spell\",\"element\":\"Water\"},{\"component_type\":\"CritComponent\", \"chance\": 0.75 }]}, {\"id\":\"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\", \"params\":[\"Spell\",\"Spell\",\"Normal\", 45]}, {\"id\":\"2508bf5c-20d7-43b4-8c77-bc677decadef\", \"params\":[\"Elf\", \"Monster\",\"Fire\", 25],\"components\":[{\"component_type\":\"ImmuneToComponent\",\"name\":\"Dragon\"}]}]"
curl --http0.9 -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"id\":\"70962948-2bf7-44a9-9ded-8c68eeac7793\", \"params\":[\"Goblin\", \"Monster\",\"Water\",  9], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}]}, {\"id\":\"74635fae-8ad3-4295-9139-320ab89c2844\", \"params\":[\"Spell\", \"Spell\",  \"Fire\",   55]}, {\"id\":\"ce6bcaee-47e1-4011-a49e-5a4d7d4245f3\", \"params\":[\"Knight\", \"Monster\",\"Normal\", 21],\"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Spell\",\"element\":\"Water\"},{\"component_type\":\"CritComponent\", \"chance\": 0.5  }]}, {\"id\":\"a6fde738-c65a-4b10-b400-6fef0fdb28ba\", \"params\":[\"Spell\",\"Spell\",\"Fire\",   55]}, {\"id\":\"a1618f1e-4f4c-4e09-9647-87e16f1edd2d\", \"params\":[\"Elf\", \"Monster\",\"Fire\", 23],\"components\":[{\"component_type\":\"ImmuneToComponent\",\"name\":\"Dragon\"}]}]"
curl --http0.9 -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"id\":\"2272ba48-6662-404d-a9a1-41a9bed316d9\", \"params\":[\"Goblin\", \"Monster\",\"Water\", 11], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}]}, {\"id\":\"3871d45b-b630-4a0d-8bc6-a5fc56b6a043\", \"params\":[\"Dragon\",\"Monster\",\"Normal\", 70]}, {\"id\":\"166c1fd5-4dcb-41a8-91cb-f45dcd57cef3\", \"params\":[\"Knight\", \"Monster\",\"Normal\", 22],\"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Spell\",\"element\":\"Water\"},{\"component_type\":\"CritComponent\", \"chance\": 0.25 }]}, {\"id\":\"237dbaef-49e3-4c23-b64b-abf5c087b276\", \"params\":[\"Spell\",\"Spell\",\"Water\",  40]}, {\"id\":\"27051a20-8580-43ff-a473-e986b52f297a\", \"params\":[\"Elf\", \"Monster\",\"Fire\", 28],\"components\":[{\"component_type\":\"ImmuneToComponent\",\"name\":\"Dragon\"}]}]"

pause

REM --------------------------------------------------
echo 7) acquire newly created packages altenhof
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
pause
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
pause
echo should fail (no money):
curl --http0.9 -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
pause
pause

REM --------------------------------------------------
echo 8) show all acquired cards kienboec
curl --http0.9 -X GET http://localhost:10001/cards --header "Authorization: Bearer kienboec-mtcgToken"
echo should fail (no token)
curl --http0.9 -X GET http://localhost:10001/cards 
pause
pause

REM --------------------------------------------------
echo 9) show all acquired cards altenhof
curl --http0.9 -X GET http://localhost:10001/cards --header "Authorization: Bearer altenhof-mtcgToken"
pause
pause

REM --------------------------------------------------
echo 10) show unconfigured deck
curl --http0.9 -X GET http://localhost:10001/deck --header "Authorization: Bearer kienboec-mtcgToken"
pause
curl --http0.9 -X GET http://localhost:10001/deck --header "Authorization: Bearer altenhof-mtcgToken"
pause
pause

REM --------------------------------------------------
echo 11) configure deck
curl --http0.9 -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]"
pause
curl --http0.9 -X GET http://localhost:10001/deck --header "Authorization: Bearer kienboec-mtcgToken"
pause
curl --http0.9 -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]"
pause
curl --http0.9 -X GET http://localhost:10001/deck --header "Authorization: Bearer altenhof-mtcgToken"
pause
pause
echo should fail and show original from before:
curl --http0.9 -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]"
pause
curl --http0.9 -X GET http://localhost:10001/deck --header "Authorization: Bearer altenhof-mtcgToken"
pause
pause
echo should fail ... only 3 cards set
curl --http0.9 -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\"]"
pause

REM --------------------------------------------------
echo 12) show configured deck 
curl --http0.9 -X GET http://localhost:10001/deck --header "Authorization: Bearer kienboec-mtcgToken"
pause
curl --http0.9 -X GET http://localhost:10001/deck --header "Authorization: Bearer altenhof-mtcgToken"
pause
pause

REM --------------------------------------------------
echo 13) show configured deck different representation
echo kienboec
curl --http0.9 -X GET http://localhost:10001/deck?format=plain --header "Authorization: Bearer kienboec-mtcgToken"
pause
pause
echo altenhof
curl --http0.9 -X GET http://localhost:10001/deck?format=plain --header "Authorization: Bearer altenhof-mtcgToken"
pause
pause

REM --------------------------------------------------
echo 14) edit user data
pause
curl --http0.9 -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
pause
curl --http0.9 -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
pause
curl --http0.9 -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Name\": \"Kienboeck\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
pause
curl --http0.9 -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Name\": \"Altenhofer\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
pause
curl --http0.9 -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
pause
curl --http0.9 -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
pause
pause
echo should fail:
curl --http0.9 -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer kienboec-mtcgToken"
pause
curl --http0.9 -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer altenhof-mtcgToken"
pause
curl --http0.9 -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Name\": \"Hoax\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
pause
curl --http0.9 -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Name\": \"Hoax\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
pause
curl --http0.9 -X GET http://localhost:10001/users/someGuy  --header "Authorization: Bearer kienboec-mtcgToken"
pause
pause

REM --------------------------------------------------
echo 15) stats
curl --http0.9 -X GET http://localhost:10001/stats --header "Authorization: Bearer kienboec-mtcgToken"
pause
curl --http0.9 -X GET http://localhost:10001/stats --header "Authorization: Bearer altenhof-mtcgToken"
pause
pause

REM --------------------------------------------------
echo 16) scoreboard
curl --http0.9 -X GET http://localhost:10001/score --header "Authorization: Bearer kienboec-mtcgToken"
pause
pause

REM --------------------------------------------------
echo 17) battle
echo noone challenging
curl --http0.9 -X GET http://localhost:10001/battles --header "Authorization: Bearer altenhof-mtcgToken"
pause
curl --http0.9 -X POST http://localhost:10001/battles --header "Authorization: Bearer altenhof-mtcgToken" -d "kienboec"
pause

echo challenge created
curl --http0.9 -X PUT http://localhost:10001/battles --header "Authorization: Bearer kienboec-mtcgToken"
pause
curl --http0.9 -X GET http://localhost:10001/battles --header "Authorization: Bearer altenhof-mtcgToken"
pause

echo challenge of friend
curl --http0.9 -X GET http://localhost:10001/battles/friends --header "Authorization: Bearer altenhof-mtcgToken"
pause
curl --http0.9 -X PUT http://localhost:10001/friends --header "Authorization: Bearer altenhof-mtcgToken" -d "kienboec"
pause
curl --http0.9 -X GET http://localhost:10001/battles/friends --header "Authorization: Bearer altenhof-mtcgToken"
pause

echo removing friend
curl --http0.9 -X DELETE http://localhost:10001/friends/kienboec --header "Authorization: Bearer altenhof-mtcgToken"
pause
curl --http0.9 -X GET http://localhost:10001/battles/friends --header "Authorization: Bearer altenhof-mtcgToken"
pause

echo battle
curl --http0.9 -X POST http://localhost:10001/battles --header "Authorization: Bearer altenhof-mtcgToken" -d "kienboec"
pause

REM --------------------------------------------------
echo 18) Stats 
echo kienboec
curl --http0.9 -X GET http://localhost:10001/stats --header "Authorization: Bearer kienboec-mtcgToken"
pause
echo altenhof
curl --http0.9 -X GET http://localhost:10001/stats --header "Authorization: Bearer altenhof-mtcgToken"
pause
pause
