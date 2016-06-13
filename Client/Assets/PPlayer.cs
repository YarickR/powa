﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PPlayer
{
	public int Money 		{ get; set;	}
	public int PlayerId		{ get; set;	}
	public int GlobalId 	{ get; set; }
	public int EOTLevel 	{ get; set; }
	public float EOMTS 		{ get; set; }
	public Color BaseColor 	{ get; set; }
	public string Name 		{ get; set; }
	public List<GameObject> Units;
	public static readonly string[] PlayerFirstNames = {
	  "James",
	  "John",
	  "Robert",
	  "Michael",
	  "William",
	  "David",
	  "Richard",
	  "Charles",
	  "Joseph",
	  "Thomas",
	  "Christopher",
	  "Daniel",
	  "Paul",
	  "Mark",
	  "Donald",
	  "George",
	  "Kenneth",
	  "Steven",
	  "Edward",
	  "Brian",
	  "Ronald",
	  "Anthony",
	  "Kevin",
	  "Jason",
	  "Matthew",
	  "Gary",
	  "Timothy",
	  "Jose",
	  "Larry",
	  "Jeffrey",
	  "Frank",
	  "Scott",
	  "Eric",
	  "Stephen",
	  "Andrew",
	  "Raymond",
	  "Gregory",
	  "Joshua",
	  "Jerry",
	  "Dennis",
	  "Walter",
	  "Patrick",
	  "Peter",
	  "Harold",
	  "Douglas",
	  "Henry",
	  "Carl",
	  "Arthur",
	  "Ryan",
	  "Roger",
	  "Joe",
	  "Juan",
	  "Jack",
	  "Albert",
	  "Jonathan",
	  "Justin",
	  "Terry",
	  "Gerald",
	  "Keith",
	  "Samuel",
	  "Willie",
	  "Ralph",
	  "Lawrence",
	  "Nicholas",
	  "Roy",
	  "Benjamin",
	  "Bruce",
	  "Brandon",
	  "Adam",
	  "Harry",
	  "Fred",
	  "Wayne",
	  "Billy",
	  "Steve",
	  "Louis",
	  "Jeremy",
	  "Aaron",
	  "Randy",
	  "Howard",
	  "Eugene",
	  "Carlos",
	  "Russell",
	  "Bobby",
	  "Victor",
	  "Martin",
	  "Ernest",
	  "Phillip",
	  "Todd",
	  "Jesse",
	  "Craig",
	  "Alan",
	  "Shawn",
	  "Clarence",
	  "Sean",
	  "Philip",
	  "Chris",
	  "Johnny",
	  "Earl",
	  "Jimmy",
	  "Antonio",
	  "Danny",
	  "Bryan",
	  "Tony",
	  "Luis",
	  "Mike",
	  "Stanley",
	  "Leonard",
	  "Nathan",
	  "Dale",
	  "Manuel",
	  "Rodney",
	  "Curtis",
	  "Norman",
	  "Allen",
	  "Marvin",
	  "Vincent",
	  "Glenn",
	  "Jeffery",
	  "Travis",
	  "Jeff",
	  "Chad",
	  "Jacob",
	  "Lee",
	  "Melvin",
	  "Alfred",
	  "Kyle",
	  "Francis",
	  "Bradley",
	  "Jesus",
	  "Herbert",
	  "Frederick",
	  "Ray",
	  "Joel",
	  "Edwin",
	  "Don",
	  "Eddie",
	  "Ricky",
	  "Troy",
	  "Randall",
	  "Barry",
	  "Alexander",
	  "Bernard",
	  "Mario",
	  "Leroy",
	  "Francisco",
	  "Marcus",
	  "Micheal",
	  "Theodore",
	  "Clifford",
	  "Miguel",
	  "Oscar",
	  "Jay",
	  "Jim",
	  "Tom",
	  "Calvin",
	  "Alex",
	  "Jon",
	  "Ronnie",
	  "Bill",
	  "Lloyd",
	  "Tommy",
	  "Leon",
	  "Derek",
	  "Warren",
	  "Darrell",
	  "Jerome",
	  "Floyd",
	  "Leo",
	  "Alvin",
	  "Tim",
	  "Wesley",
	  "Gordon",
	  "Dean",
	  "Greg",
	  "Jorge",
	  "Dustin",
	  "Pedro",
	  "Derrick",
	  "Dan",
	  "Lewis",
	  "Zachary",
	  "Corey",
	  "Herman",
	  "Maurice",
	  "Vernon",
	  "Roberto",
	  "Clyde",
	  "Glen",
	  "Hector",
	  "Shane",
	  "Ricardo",
	  "Sam",
	  "Rick",
	  "Lester",
	  "Brent",
	  "Ramon",
	  "Charlie",
	  "Tyler",
	  "Gilbert",
	  "Gene",
	  "Marc",
	  "Reginald",
	  "Ruben",
	  "Brett",
	  "Angel",
	  "Nathaniel",
	  "Rafael",
	  "Leslie",
	  "Edgar",
	  "Milton",
	  "Raul",
	  "Ben",
	  "Chester",
	  "Cecil",
	  "Duane",
	  "Franklin",
	  "Andre",
	  "Elmer",
	  "Brad",
	  "Gabriel",
	  "Ron",
	  "Mitchell",
	  "Roland",
	  "Arnold",
	  "Harvey",
	  "Jared",
	  "Adrian",
	  "Karl",
	  "Cory",
	  "Claude",
	  "Erik",
	  "Darryl",
	  "Jamie",
	  "Neil",
	  "Jessie",
	  "Christian",
	  "Javier",
	  "Fernando",
	  "Clinton",
	  "Ted",
	  "Mathew",
	  "Tyrone",
	  "Darren",
	  "Lonnie",
	  "Lance",
	  "Cody",
	  "Julio",
	  "Kelly",
	  "Kurt",
	  "Allan",
	  "Nelson",
	  "Guy",
	  "Clayton",
	  "Hugh",
	  "Max",
	  "Dwayne",
	  "Dwight",
	  "Armando",
	  "Felix",
	  "Jimmie",
	  "Everett",
	  "Jordan",
	  "Ian",
	  "Wallace",
	  "Ken",
	  "Bob",
	  "Jaime",
	  "Casey",
	  "Alfredo",
	  "Alberto",
	  "Dave",
	  "Ivan",
	  "Johnnie",
	  "Sidney",
	  "Byron",
	  "Julian",
	  "Isaac",
	  "Morris",
	  "Clifton",
	  "Willard",
	  "Daryl",
	  "Ross",
	  "Virgil",
	  "Andy",
	  "Marshall",
	  "Salvador",
	  "Perry",
	  "Kirk",
	  "Sergio",
	  "Marion",
	  "Tracy",
	  "Seth",
	  "Kent",
	  "Terrance",
	  "Rene",
	  "Eduardo",
	  "Terrence",
	  "Enrique",
	  "Freddie",
	  "Wade",
	  "Austin",
	  "Stuart",
	  "Fredrick",
	  "Arturo",
	  "Alejandro",
	  "Jackie",
	  "Joey",
	  "Nick",
	  "Luther",
	  "Wendell",
	  "Jeremiah",
	  "Evan",
	  "Julius",
	  "Dana",
	  "Donnie",
	  "Otis",
	  "Shannon",
	  "Trevor",
	  "Oliver",
	  "Luke",
	  "Homer",
	  "Gerard",
	  "Doug",
	  "Kenny",
	  "Hubert",
	  "Angelo",
	  "Shaun",
	  "Lyle",
	  "Matt",
	  "Lynn",
	  "Alfonso",
	  "Orlando",
	  "Rex",
	  "Carlton",
	  "Ernesto",
	  "Cameron",
	  "Neal",
	  "Pablo",
	  "Lorenzo",
	  "Omar",
	  "Wilbur",
	  "Blake",
	  "Grant",
	  "Horace",
	  "Roderick",
	  "Kerry",
	  "Abraham",
	  "Willis",
	  "Rickey",
	  "Jean",
	  "Ira",
	  "Andres",
	  "Cesar",
	  "Johnathan",
	  "Malcolm",
	  "Rudolph",
	  "Damon",
	  "Kelvin",
	  "Rudy",
	  "Preston",
	  "Alton",
	  "Archie",
	  "Marco",
	  "Wm",
	  "Pete",
	  "Randolph",
	  "Garry",
	  "Geoffrey",
	  "Jonathon",
	  "Felipe",
	  "Bennie",
	  "Gerardo",
	  "Ed",
	  "Dominic",
	  "Robin",
	  "Loren",
	  "Delbert",
	  "Colin",
	  "Guillermo",
	  "Earnest",
	  "Lucas",
	  "Benny",
	  "Noel",
	  "Spencer",
	  "Rodolfo",
	  "Myron",
	  "Edmund",
	  "Garrett",
	  "Salvatore",
	  "Cedric",
	  "Lowell",
	  "Gregg",
	  "Sherman",
	  "Wilson",
	  "Devin",
	  "Sylvester",
	  "Kim",
	  "Roosevelt",
	  "Israel",
	  "Jermaine",
	  "Forrest",
	  "Wilbert",
	  "Leland",
	  "Simon",
	  "Guadalupe",
	  "Clark",
	  "Irving",
	  "Carroll",
	  "Bryant",
	  "Owen",
	  "Rufus",
	  "Woodrow",
	  "Sammy",
	  "Kristopher",
	  "Mack",
	  "Levi",
	  "Marcos",
	  "Gustavo",
	  "Jake",
	  "Lionel",
	  "Marty",
	  "Taylor",
	  "Ellis",
	  "Dallas",
	  "Gilberto",
	  "Clint",
	  "Nicolas",
	  "Laurence",
	  "Ismael",
	  "Orville",
	  "Drew",
	  "Jody",
	  "Ervin",
	  "Dewey",
	  "Al",
	  "Wilfred",
	  "Josh",
	  "Hugo",
	  "Ignacio",
	  "Caleb",
	  "Tomas",
	  "Sheldon",
	  "Erick",
	  "Frankie",
	  "Stewart",
	  "Doyle",
	  "Darrel",
	  "Rogelio",
	  "Terence",
	  "Santiago",
	  "Alonzo",
	  "Elias",
	  "Bert",
	  "Elbert",
	  "Ramiro",
	  "Conrad",
	  "Pat",
	  "Noah",
	  "Grady",
	  "Phil",
	  "Cornelius",
	  "Lamar",
	  "Rolando",
	  "Clay",
	  "Percy",
	  "Dexter",
	  "Bradford",
	  "Merle",
	  "Darin",
	  "Amos",
	  "Terrell",
	  "Moses",
	  "Irvin",
	  "Saul",
	  "Roman",
	  "Darnell",
	  "Randal",
	  "Tommie",
	  "Timmy",
	  "Darrin",
	  "Winston",
	  "Brendan",
	  "Toby",
	  "Van",
	  "Abel",
	  "Dominick",
	  "Boyd",
	  "Courtney",
	  "Jan",
	  "Emilio",
	  "Elijah",
	  "Cary",
	  "Domingo",
	  "Santos",
	  "Aubrey",
	  "Emmett",
	  "Marlon",
	  "Emanuel",
	  "Jerald",
	  "Edmond",
	  "Emil",
	  "Dewayne",
	  "Will",
	  "Otto",
	  "Teddy",
	  "Reynaldo",
	  "Bret",
	  "Morgan",
	  "Jess",
	  "Trent",
	  "Humberto",
	  "Emmanuel",
	  "Stephan",
	  "Louie",
	  "Vicente",
	  "Lamont",
	  "Stacy",
	  "Garland",
	  "Miles",
	  "Micah",
	  "Efrain",
	  "Billie",
	  "Logan",
	  "Heath",
	  "Rodger",
	  "Harley",
	  "Demetrius",
	  "Ethan",
	  "Eldon",
	  "Rocky",
	  "Pierre",
	  "Junior",
	  "Freddy",
	  "Eli",
	  "Bryce",
	  "Antoine",
	  "Robbie",
	  "Kendall",
	  "Royce",
	  "Sterling",
	  "Mickey",
	  "Chase",
	  "Grover",
	  "Elton",
	  "Cleveland",
	  "Dylan",
	  "Chuck",
	  "Damian",
	  "Reuben",
	  "Stan",
	  "August",
	  "Leonardo",
	  "Jasper",
	  "Russel",
	  "Erwin",
	  "Benito",
	  "Hans",
	  "Monte",
	  "Blaine",
	  "Ernie",
	  "Curt",
	  "Quentin",
	  "Agustin",
	  "Murray",
	  "Jamal",
	  "Devon",
	  "Adolfo",
	  "Harrison",
	  "Tyson",
	  "Burton",
	  "Brady",
	  "Elliott",
	  "Wilfredo",
	  "Bart",
	  "Jarrod",
	  "Vance",
	  "Denis",
	  "Damien",
	  "Joaquin",
	  "Harlan",
	  "Desmond",
	  "Elliot",
	  "Darwin",
	  "Ashley",
	  "Gregorio",
	  "Buddy",
	  "Xavier",
	  "Kermit",
	  "Roscoe",
	  "Esteban",
	  "Anton",
	  "Solomon",
	  "Scotty",
	  "Norbert",
	  "Elvin",
	  "Williams",
	  "Nolan",
	  "Carey",
	  "Rod",
	  "Quinton",
	  "Hal",
	  "Brain",
	  "Rob",
	  "Elwood",
	  "Kendrick",
	  "Darius",
	  "Moises",
	  "Son",
	  "Marlin",
	  "Fidel",
	  "Thaddeus",
	  "Cliff",
	  "Marcel",
	  "Ali",
	  "Jackson",
	  "Raphael",
	  "Bryon",
	  "Armand",
	  "Alvaro",
	  "Jeffry",
	  "Dane",
	  "Joesph",
	  "Thurman",
	  "Ned",
	  "Sammie",
	  "Rusty",
	  "Michel",
	  "Monty",
	  "Rory",
	  "Fabian",
	  "Reggie",
	  "Mason",
	  "Graham",
	  "Kris",
	  "Isaiah",
	  "Vaughn",
	  "Gus",
	  "Avery",
	  "Loyd",
	  "Diego",
	  "Alexis",
	  "Adolph",
	  "Norris",
	  "Millard",
	  "Rocco",
	  "Gonzalo",
	  "Derick",
	  "Rodrigo",
	  "Gerry",
	  "Stacey",
	  "Carmen",
	  "Wiley",
	  "Rigoberto",
	  "Alphonso",
	  "Ty",
	  "Shelby",
	  "Rickie",
	  "Noe",
	  "Vern",
	  "Bobbie",
	  "Reed",
	  "Jefferson",
	  "Elvis",
	  "Bernardo",
	  "Mauricio",
	  "Hiram",
	  "Donovan",
	  "Basil",
	  "Riley",
	  "Ollie",
	  "Nickolas",
	  "Maynard",
	  "Scot",
	  "Vince",
	  "Quincy",
	  "Eddy",
	  "Sebastian",
	  "Federico",
	  "Ulysses",
	  "Heriberto",
	  "Donnell",
	  "Cole",
	  "Denny",
	  "Davis",
	  "Gavin",
	  "Emery",
	  "Ward",
	  "Romeo",
	  "Jayson",
	  "Dion",
	  "Dante",
	  "Clement",
	  "Coy",
	  "Odell",
	  "Maxwell",
	  "Jarvis",
	  "Bruno",
	  "Issac",
	  "Mary",
	  "Dudley",
	  "Brock",
	  "Sanford",
	  "Colby",
	  "Carmelo",
	  "Barney",
	  "Nestor",
	  "Hollis",
	  "Stefan",
	  "Donny",
	  "Art",
	  "Linwood",
	  "Beau",
	  "Weldon",
	  "Galen",
	  "Isidro",
	  "Truman",
	  "Delmar",
	  "Johnathon",
	  "Silas",
	  "Frederic",
	  "Dick",
	  "Kirby",
	  "Irwin",
	  "Cruz",
	  "Merlin",
	  "Merrill",
	  "Charley",
	  "Marcelino",
	  "Lane",
	  "Harris",
	  "Cleo",
	  "Carlo",
	  "Trenton",
	  "Kurtis",
	  "Hunter",
	  "Aurelio",
	  "Winfred",
	  "Vito",
	  "Collin",
	  "Denver",
	  "Carter",
	  "Leonel",
	  "Emory",
	  "Pasquale",
	  "Mohammad",
	  "Mariano",
	  "Danial",
	  "Blair",
	  "Landon",
	  "Dirk",
	  "Branden",
	  "Adan",
	  "Numbers",
	  "Clair",
	  "Buford",
	  "German",
	  "Bernie",
	  "Wilmer",
	  "Joan",
	  "Emerson",
	  "Zachery",
	  "Fletcher",
	  "Jacques",
	  "Errol",
	  "Dalton",
	  "Monroe",
	  "Josue",
	  "Dominique",
	  "Edwardo",
	  "Booker",
	  "Wilford",
	  "Sonny",
	  "Shelton",
	  "Carson",
	  "Theron",
	  "Raymundo",
	  "Daren",
	  "Tristan",
	  "Houston",
	  "Robby",
	  "Lincoln",
	  "Jame",
	  "Genaro",
	  "Gale",
	  "Bennett",
	  "Octavio",
	  "Cornell",
	  "Laverne",
	  "Hung",
	  "Arron",
	  "Antony",
	  "Herschel",
	  "Alva",
	  "Giovanni",
	  "Garth",
	  "Cyrus",
	  "Cyril",
	  "Ronny",
	  "Stevie",
	  "Lon",
	  "Freeman",
	  "Erin",
	  "Duncan",
	  "Kennith",
	  "Carmine",
	  "Augustine",
	  "Young",
	  "Erich",
	  "Chadwick",
	  "Wilburn",
	  "Russ",
	  "Reid",
	  "Myles",
	  "Anderson",
	  "Morton",
	  "Jonas",
	  "Forest",
	  "Mitchel",
	  "Mervin",
	  "Zane",
	  "Rich",
	  "Jamel",
	  "Lazaro",
	  "Alphonse",
	  "Randell",
	  "Major",
	  "Johnie",
	  "Jarrett",
	  "Brooks",
	  "Ariel",
	  "Abdul",
	  "Dusty",
	  "Luciano",
	  "Lindsey",
	  "Tracey",
	  "Seymour",
	  "Scottie",
	  "Eugenio",
	  "Mohammed",
	  "Sandy",
	  "Valentin",
	  "Chance",
	  "Arnulfo",
	  "Lucien",
	  "Ferdinand",
	  "Thad",
	  "Ezra",
	  "Sydney",
	  "Aldo",
	  "Rubin",
	  "Royal",
	  "Mitch",
	  "Earle",
	  "Abe",
	  "Wyatt",
	  "Marquis",
	  "Lanny",
	  "Kareem",
	  "Jamar",
	  "Boris",
	  "Isiah",
	  "Emile",
	  "Elmo",
	  "Aron",
	  "Leopoldo",
	  "Everette",
	  "Josef",
	  "Gail",
	  "Eloy",
	  "Dorian",
	  "Rodrick",
	  "Reinaldo",
	  "Lucio",
	  "Jerrod",
	  "Weston",
	  "Hershel",
	  "Barton",
	  "Parker",
	  "Lemuel",
	  "Lavern",
	  "Burt",
	  "Jules",
	  "Gil",
	  "Eliseo",
	  "Ahmad",
	  "Nigel",
	  "Efren",
	  "Antwan",
	  "Alden",
	  "Margarito",
	  "Coleman",
	  "Refugio",
	  "Dino",
	  "Osvaldo",
	  "Les",
	  "Deandre",
	  "Normand",
	  "Kieth",
	  "Ivory",
	  "Andrea",
	  "Trey",
	  "Norberto",
	  "Napoleon",
	  "Jerold",
	  "Fritz",
	  "Rosendo",
	  "Milford",
	  "Sang",
	  "Deon",
	  "Christoper",
	  "Alfonzo",
	  "Lyman",
	  "Josiah",
	  "Brant",
	  "Wilton",
	  "Rico",
	  "Jamaal",
	  "Dewitt",
	  "Carol",
	  "Brenton",
	  "Yong",
	  "Olin",
	  "Foster",
	  "Faustino",
	  "Claudio",
	  "Judson",
	  "Gino",
	  "Edgardo",
	  "Berry",
	  "Alec",
	  "Tanner",
	  "Jarred",
	  "Donn",
	  "Trinidad",
	  "Tad",
	  "Shirley",
	  "Prince",
	  "Porfirio",
	  "Odis",
	  "Maria",
	  "Lenard",
	  "Chauncey",
	  "Chang",
	  "Tod",
	  "Mel",
	  "Marcelo",
	  "Kory",
	  "Augustus",
	  "Keven",
	  "Hilario",
	  "Bud",
	  "Sal",
	  "Rosario",
	  "Orval",
	  "Mauro",
	  "Dannie",
	  "Zachariah",
	  "Olen",
	  "Anibal",
	  "Milo",
	  "Jed",
	  "Frances",
	  "Thanh",
	  "Dillon",
	  "Amado",
	  "Newton",
	  "Connie",
	  "Lenny",
	  "Tory",
	  "Richie",
	  "Lupe",
	  "Horacio",
	  "Brice",
	  "Mohamed",
	  "Delmer",
	  "Dario",
	  "Reyes",
	  "Dee",
	  "Mac",
	  "Jonah",
	  "Jerrold",
	  "Robt",
	  "Hank",
	  "Sung",
	  "Rupert",
	  "Rolland",
	  "Kenton",
	  "Damion",
	  "Chi",
	  "Antone",
	  "Waldo",
	  "Fredric",
	  "Bradly",
	  "Quinn",
	  "Kip",
	  "Burl",
	  "Walker",
	  "Tyree",
	  "Jefferey",
	  "Ahmed",
	  "Willy",
	  "Stanford",
	  "Oren",
	  "Noble",
	  "Moshe",
	  "Mikel",
	  "Enoch",
	  "Brendon",
	  "Quintin",
	  "Jamison",
	  "Florencio",
	  "Darrick",
	  "Tobias",
	  "Minh",
	  "Hassan",
	  "Giuseppe",
	  "Demarcus",
	  "Cletus",
	  "Tyrell",
	  "Lyndon",
	  "Keenan",
	  "Werner",
	  "Theo",
	  "Geraldo",
	  "Lou",
	  "Columbus",
	  "Chet",
	  "Bertram",
	  "Markus",
	  "Huey",
	  "Hilton",
	  "Dwain",
	  "Donte",
	  "Tyron",
	  "Omer",
	  "Isaias",
	  "Hipolito",
	  "Fermin",
	  "Chung",
	  "Adalberto",
	  "Valentine",
	  "Jamey",
	  "Bo",
	  "Barrett",
	  "Whitney",
	  "Teodoro",
	  "Mckinley",
	  "Maximo",
	  "Garfield",
	  "Sol",
	  "Raleigh",
	  "Lawerence",
	  "Abram",
	  "Rashad",
	  "King",
	  "Emmitt",
	  "Daron",
	  "Chong",
	  "Samual",
	  "Paris",
	  "Otha",
	  "Miquel",
	  "Lacy",
	  "Eusebio",
	  "Dong",
	  "Domenic",
	  "Darron",
	  "Buster",
	  "Antonia",
	  "Wilber",
	  "Renato",
	  "Jc",
	  "Hoyt",
	  "Haywood",
	  "Ezekiel",
	  "Chas",
	  "Florentino",
	  "Elroy",
	  "Clemente",
	  "Arden",
	  "Neville",
	  "Kelley",
	  "Edison",
	  "Deshawn",
	  "Carrol",
	  "Shayne",
	  "Nathanial",
	  "Jordon",
	  "Danilo",
	  "Claud",
	  "Val",
	  "Sherwood",
	  "Raymon",
	  "Rayford",
	  "Cristobal",
	  "Ambrose",
	  "Titus",
	  "Hyman",
	  "Felton",
	  "Ezequiel",
	  "Erasmo",
	  "Stanton",
	  "Lonny",
	  "Len",
	  "Ike",
	  "Milan",
	  "Lino",
	  "Jarod",
	  "Herb",
	  "Andreas",
	  "Walton",
	  "Rhett",
	  "Palmer",
	  "Jude",
	  "Douglass",
	  "Cordell",
	  "Oswaldo",
	  "Ellsworth",
	  "Virgilio",
	  "Toney",
	  "Nathanael",
	  "Del",
	  "Britt",
	  "Benedict",
	  "Mose",
	  "Hong",
	  "Leigh",
	  "Johnson",
	  "Isreal",
	  "Gayle",
	  "Garret",
	  "Fausto",
	  "Asa",
	  "Arlen",
	  "Zack",
	  "Warner",
	  "Modesto",
	  "Francesco",
	  "Manual",
	  "Jae",
	  "Gaylord",
	  "Gaston",
	  "Filiberto",
	  "Deangelo",
	  "Michale",
	  "Granville",
	  "Wes",
	  "Malik",
	  "Zackary",
	  "Tuan",
	  "Nicky",
	  "Eldridge",
	  "Cristopher",
	  "Cortez",
	  "Antione",
	  "Malcom",
	  "Long",
	  "Korey",
	  "Jospeh",
	  "Colton",
	  "Waylon",
	  "Von",
	  "Hosea",
	  "Shad",
	  "Santo",
	  "Rudolf",
	  "Rolf",
	  "Rey",
	  "Renaldo",
	  "Marcellus",
	  "Lucius",
	  "Lesley",
	  "Kristofer",
	  "Boyce",
	  "Benton",
	  "Man",
	  "Kasey",
	  "Jewell",
	  "Hayden",
	  "Harland",
	  "Arnoldo",
	  "Rueben",
	  "Leandro",
	  "Kraig",
	  "Jerrell",
	  "Jeromy",
	  "Hobert",
	  "Cedrick",
	  "Arlie",
	  "Winford",
	  "Wally",
	  "Patricia",
	  "Luigi",
	  "Keneth",
	  "Jacinto",
	  "Graig",
	  "Franklyn",
	  "Edmundo",
	  "Sid",
	  "Porter",
	  "Leif",
	  "Lauren",
	  "Jeramy",
	  "Elisha",
	  "Buck",
	  "Willian",
	  "Vincenzo",
	  "Shon",
	  "Michal",
	  "Lynwood",
	  "Lindsay",
	  "Jewel",
	  "Jere",
	  "Hai",
	  "Elden",
	  "Dorsey",
	  "Darell",
	  "Broderick",
	  "Alonso"
	};

	public static readonly string[] PlayerLastNames = {
	  "Smith",
	  "Brown",
	  "Johnson",
	  "Jones",
	  "Williams",
	  "Davis",
	  "Miller",
	  "Wilson",
	  "Taylor",
	  "Clark",
	  "White",
	  "Moore",
	  "Thompson",
	  "Allen",
	  "Martin",
	  "Hall",
	  "Adams",
	  "Thomas",
	  "Wright",
	  "Baker",
	  "Walker",
	  "Anderson",
	  "Lewis",
	  "Harris",
	  "Hill",
	  "King",
	  "Jackson",
	  "Lee",
	  "Green",
	  "Wood",
	  "Parker",
	  "Campbell",
	  "Young",
	  "Robinson",
	  "Stewart",
	  "Scott",
	  "Rogers",
	  "Roberts",
	  "Cook",
	  "Phillips",
	  "Turner",
	  "Carter",
	  "Ward",
	  "Foster",
	  "Morgan",
	  "Howard",
	  "Cox",
	  "Jr",
	  "Bailey",
	  "Richardson",
	  "Reed",
	  "Russell",
	  "Edwards",
	  "Morris",
	  "Wells",
	  "Palmer",
	  "Ann",
	  "Mitchell",
	  "Evans",
	  "Gray",
	  "Wheeler",
	  "Warren",
	  "Cooper",
	  "Bell",
	  "Collins",
	  "Carpenter",
	  "Stone",
	  "Cole",
	  "Ellis",
	  "Bennett",
	  "Harrison",
	  "Fisher",
	  "Henry",
	  "Spencer",
	  "Watson",
	  "Porter",
	  "Nelson",
	  "James",
	  "Marshall",
	  "Butler",
	  "Hamilton",
	  "Tucker",
	  "Stevens",
	  "Webb",
	  "May",
	  "West",
	  "Reynolds",
	  "Hunt",
	  "Barnes",
	  "Perkins",
	  "Brooks",
	  "Long",
	  "Price",
	  "Fuller",
	  "Powell",
	  "Perry",
	  "Alexander",
	  "Rice",
	  "Hart",
	  "Ross",
	  "Arnold",
	  "Shaw",
	  "Ford",
	  "Pierce",
	  "Lawrence",
	  "Henderson",
	  "Freeman",
	  "Mason",
	  "Andrews",
	  "Graham",
	  "Chapman",
	  "Hughes",
	  "Mills",
	  "Gardner",
	  "Jordan",
	  "Ball",
	  "Nichols",
	  "Gibson",
	  "Greene",
	  "Wallace",
	  "Baldwin",
	  "Day",
	  "Deaver",
	  "Sherman",
	  "Murphy",
	  "Lane",
	  "Knight",
	  "Holmes",
	  "Bishop",
	  "Kelly",
	  "French",
	  "Myers",
	  "Mentioned",
	  "Patterson",
	  "Elizabeth",
	  "Case",
	  "Curtis",
	  "Simmons",
	  "Jenkins",
	  "Berry",
	  "Hopkins",
	  "Clarke",
	  "Fox",
	  "Gordon",
	  "Hunter",
	  "Robertson",
	  "Payne",
	  "Johnston",
	  "Barker",
	  "Grant",
	  "Murray",
	  "Church",
	  "Webster",
	  "Richards",
	  "Sanders",
	  "Page",
	  "Crawford",
	  "Duncan",
	  "Warner",
	  "Hale",
	  "Kennedy",
	  "Rose",
	  "Carr",
	  "Black",
	  "Bates",
	  "Chase",
	  "Pratt",
	  "Austin",
	  "Hawkins",
	  "Stephens",
	  "Ferguson",
	  "Parsons",
	  "Simpson",
	  "Armstrong",
	  "Fowler",
	  "Potter",
	  "Hayes",
	  "Griffin",
	  "Bryant",
	  "Weaver",
	  "Boyd",
	  "Townsend",
	  "Coleman",
	  "Holland",
	  "Stanley",
	  "Hicks",
	  "Gilbert",
	  "Bradley",
	  "Chandler",
	  "Barber",
	  "Bartlett",
	  "Woods",
	  "Sutton",
	  "Montgomery",
	  "Dean",
	  "Morse",
	  "Brewer",
	  "Newton",
	  "Sullivan",
	  "Jane",
	  "Graves",
	  "Phelps",
	  "Hubbard",
	  "Fletcher",
	  "Drake",
	  "Douglas",
	  "Dunn",
	  "Burton",
	  "Sharp",
	  "Mcdonald",
	  "Elliott",
	  "Eaton",
	  "Harvey",
	  "Peterson",
	  "Franklin",
	  "Morrison",
	  "George",
	  "Lincoln",
	  "Snyder",
	  "Hudson",
	  "Snow",
	  "Cobb",
	  "England",
	  "Gregory",
	  "Wilcox",
	  "Bowen",
	  "Howell",
	  "Cunningham",
	  "Bowman",
	  "Norton",
	  "Lord",
	  "Willis",
	  "Holt",
	  "Little",
	  "Williamson",
	  "Davidson",
	  "Harrington",
	  "Marsh",
	  "County",
	  "Daigle",
	  "Leonard",
	  "Harper",
	  "Dixon",
	  "Matthews",
	  "Ray",
	  "Mary",
	  "Whitney",
	  "Burns",
	  "Boone",
	  "Peck",
	  "Bradford",
	  "Owen",
	  "Garrett",
	  "Barrett",
	  "Hammond",
	  "Oliver",
	  "John",
	  "Mann",
	  "Stuart",
	  "Peters",
	  "Welch",
	  "Reeves",
	  "Hull",
	  "Caldwell",
	  "Rhodes",
	  "Howe",
	  "Owens",
	  "Gates",
	  "Bush",
	  "Pearson",
	  "Newman",
	  "Frost",
	  "Wagner",
	  "Bruce",
	  "Kimball",
	  "Abbott",
	  "Plantagenet",
	  "Robbins",
	  "Briggs",
	  "Wade",
	  "Mullins",
	  "Woodward",
	  "Stafford",
	  "Barton",
	  "Todd",
	  "Goodwin",
	  "Dyer",
	  "Horton",
	  "Watkins",
	  "Cummings",
	  "Sparks",
	  "Bacon",
	  "Gould",
	  "Sawyer",
	  "Neal",
	  "Kelley",
	  "Reid",
	  "Cooke",
	  "Osborne",
	  "Hancock",
	  "Angell",
	  "Newcomb",
	  "Hershey",
	  "Moseley",
	  "Secor",
	  "Quincy",
	  "Hinton",
	  "Holder",
	  "Priest",
	  "Travis",
	  "Granger",
	  "Gallagher",
	  "Lanier",
	  "Lutz",
	  "Simon",
	  "Cromwell",
	  "Rand",
	  "Childers",
	  "Wheelock",
	  "Hester",
	  "Knowlton",
	  "Moses",
	  "Fulton",
	  "Cecil",
	  "Platt",
	  "Doane",
	  "Self",
	  "Dell",
	  "Runyon",
	  "Allred",
	  "Tipton",
	  "Denny",
	  "Coles",
	  "Coon",
	  "Willoughby",
	  "Lynn",
	  "Munson",
	  "Schroeder",
	  "Hasbrouck",
	  "Wilkerson",
	  "Kelsey",
	  "Beall",
	  "Axtell",
	  "Roth",
	  "Stoner",
	  "Currier",
	  "Mcpherson",
	  "Wolcott",
	  "Maddox",
	  "Pond",
	  "Chastain",
	  "Fisk",
	  "Talley",
	  "Hand",
	  "Hawes",
	  "Pritchard",
	  "Simonds",
	  "Barclay",
	  "Rockwell",
	  "Dickey",
	  "Castle",
	  "Ogle",
	  "Barney",
	  "Joy",
	  "Neff",
	  "Livermore",
	  "Bowden",
	  "Patten",
	  "Bolling",
	  "Darby",
	  "Danforth",
	  "Forrest",
	  "Apr",
	  "Mcintyre",
	  "Clough",
	  "Jewell",
	  "Earle",
	  "Kenyon",
	  "Napier",
	  "Hiatt",
	  "Legg",
	  "Britton",
	  "Wetherbee",
	  "Haas",
	  "Alford",
	  "Odell",
	  "Ashby",
	  "Ash",
	  "Broyles",
	  "Erickson",
	  "Dewitt",
	  "Akers",
	  "Herndon",
	  "Branch",
	  "Mcclain",
	  "Delano",
	  "Stacy",
	  "Elkins",
	  "Byers",
	  "Friend",
	  "Zouche",
	  "Worthington",
	  "Colvin",
	  "Simons",
	  "Adair",
	  "Eliot",
	  "Quick",
	  "Keen",
	  "Grubb",
	  "Unk",
	  "How",
	  "Culver",
	  "Gaylord",
	  "Shipman",
	  "Chester",
	  "Batchelder",
	  "Taft",
	  "Berg",
	  "Bigod",
	  "Deming",
	  "Farnsworth",
	  "Chadwick",
	  "Oldham",
	  "Averill",
	  "Brownell",
	  "Hobson",
	  "Carruthers",
	  "Throckmorton",
	  "Pryor",
	  "Boucher",
	  "Meade",
	  "Ritter",
	  "Woodson",
	  "Bowles",
	  "Gagnon",
	  "Mccarthy",
	  "Helms",
	  "Frey",
	  "Ives",
	  "Barham",
	  "Casto",
	  "Peeler",
	  "Cummins",
	  "Parkhurst",
	  "Huggins",
	  "Slaughter",
	  "Hahn",
	  "Farrell",
	  "Wiseman",
	  "Ponder",
	  "Tudor",
	  "Hurley",
	  "Dees",
	  "Huddleston",
	  "Kendrick",
	  "Thorn",
	  "Duckworth",
	  "Heald",
	  "Marion",
	  "Kinney",
	  "Brandon",
	  "Groves",
	  "Brumbaugh",
	  "Ripley",
	  "Moran",
	  "Eames",
	  "Harcourt",
	  "Skaggs",
	  "Mattingly",
	  "Christensen",
	  "Kramer",
	  "Lockhart",
	  "Neil",
	  "Swanson",
	  "Albright",
	  "Nix",
	  "Gaston",
	  "Pruitt",
	  "Cramer",
	  "Donnell",
	  "Abernathy",
	  "Staples",
	  "Nicholas",
	  "Appleton",
	  "Dempsey",
	  "Flagg",
	  "Whittington",
	  "Gunter",
	  "Storm",
	  "Merrick",
	  "Moser",
	  "Newhall",
	  "Hook",
	  "Blanton",
	  "Mcmahon",
	  "Rawson",
	  "Poe",
	  "Bowling",
	  "Pack",
	  "Whitmore",
	  "Cherry",
	  "Painter",
	  "Prentice",
	  "Fitzpatrick",
	  "Vail",
	  "Tilden",
	  "Woodworth",
	  "Skelton",
	  "Congdon",
	  "Westbrook",
	  "Basham",
	  "Sands",
	  "Spangler",
	  "Abbot",
	  "Buckner",
	  "Mccall",
	  "Cogswell",
	  "Coates",
	  "Bland",
	  "Beeson",
	  "Sherrill",
	  "Libby",
	  "Loring",
	  "Dill",
	  "Hinkle",
	  "Emmons",
	  "Millard",
	  "Mooney",
	  "Higginbotham",
	  "Gamble",
	  "Mayhew",
	  "Weir",
	  "Sadler",
	  "Puckett",
	  "Corbet",
	  "Kay",
	  "Beatty",
	  "Mather",
	  "Koch",
	  "Bower",
	  "Carleton",
	  "Harwood",
	  "Henley",
	  "Purdy",
	  "Sharpe",
	  "Woodard",
	  "Larkin",
	  "Brackett",
	  "Keener",
	  "Hoag",
	  "Tull",
	  "Meredith",
	  "Southworth",
	  "Bender",
	  "Dwight",
	  "Huber",
	  "Olsen",
	  "Morrill",
	  "Kirkland",
	  "Hedges",
	  "Ratcliff",
	  "Gee",
	  "Steen",
	  "Womack",
	  "Sabin",
	  "Erwin",
	  "Cagle",
	  "Salyer",
	  "Sweeney",
	  "Heard",
	  "Bartholomew",
	  "Freer",
	  "Pickett",
	  "Steel",
	  "Kingsbury",
	  "Willson",
	  "Blodgett",
	  "Paterson",
	  "Blood",
	  "Guy",
	  "Call",
	  "Cartwright",
	  "Ashton",
	  "Bilbrey",
	  "Diehl",
	  "Beals",
	  "Wales",
	  "Marston",
	  "Daggett",
	  "Conklin",
	  "Langford",
	  "Madden",
	  "Ellsworth",
	  "Holcombe",
	  "Biggs",
	  "Nickell",
	  "Lilly",
	  "Lott",
	  "Harp",
	  "Stroud",
	  "Barnett",
	  "Waters",
	  "Field",
	  "Griffith",
	  "Bond",
	  "Washington",
	  "Craig",
	  "Meyer",
	  "Hutchinson",
	  "Blair",
	  "Steele",
	  "Blake",
	  "Strong",
	  "Morton",
	  "Norris",
	  "Francis",
	  "Chambers",
	  "Crane",
	  "Babcock",
	  "Walton",
	  "Lawson",
	  "Merrill",
	  "Jennings",
	  "Hoffman",
	  "Dodge",
	  "Preston",
	  "Lambert",
	  "Cross",
	  "Haynes",
	  "Bliss",
	  "Fleming",
	  "Tyler",
	  "Lamb",
	  "Bryan",
	  "Sprague",
	  "Dudley",
	  "Underwood",
	  "Carroll",
	  "Burgess",
	  "Pope",
	  "Saunders",
	  "Jacobs",
	  "Nash",
	  "Riley",
	  "Putnam",
	  "Bird",
	  "Neville",
	  "Daniel",
	  "Clare",
	  "Hatch",
	  "Thornton",
	  "Daniels",
	  "Maria",
	  "Browne",
	  "Farmer",
	  "Lowe",
	  "Heath",
	  "Randall",
	  "Inman",
	  "Tuttle",
	  "Garner",
	  "Ballard",
	  "Skinner",
	  "Miles",
	  "Hyde",
	  "Beck",
	  "Maxwell",
	  "Parks",
	  "Beauchamp",
	  "Terry",
	  "Powers",
	  "Davenport",
	  "Harmon",
	  "Gentry",
	  "Hardy",
	  "Ryan",
	  "Watts",
	  "Shelton",
	  "Booth",
	  "Mccoy",
	  "Yates",
	  "Sears",
	  "Lucas",
	  "Clayton",
	  "Walters",
	  "Mendenhall",
	  "Dawson",
	  "Manning",
	  "Horn",
	  "Denton",
	  "Thayer",
	  "Kent",
	  "Savage",
	  "Stout",
	  "Ramsey",
	  "Stanton",
	  "Browning",
	  "Gibbs",
	  "Hastings",
	  "Lynch",
	  "Hoover",
	  "Sims",
	  "Avery",
	  "Crosby",
	  "Moss",
	  "Mead",
	  "Buck",
	  "Keith",
	  "Bigelow",
	  "Osborn",
	  "Adkins",
	  "Schmidt",
	  "Lyon",
	  "Brewster",
	  "William",
	  "Dennis",
	  "Atkinson",
	  "Hodges",
	  "Wilkinson",
	  "Strickland",
	  "Gill",
	  "Kendall",
	  "Read",
	  "Shepherd",
	  "Harding",
	  "Moody",
	  "Fellows",
	  "Buchanan",
	  "Keller",
	  "Higgins",
	  "Howland",
	  "Pearce",
	  "Glover",
	  "Hanson",
	  "York",
	  "Lindsey",
	  "Bass",
	  "Rowe",
	  "Sargent",
	  "Stevenson",
	  "Dillon",
	  "Haines",
	  "Moon",
	  "Mcdaniel",
	  "Emerson",
	  "Winslow",
	  "Wall",
	  "Son",
	  "Grey",
	  "Camp",
	  "Blanchard",
	  "Dickinson",
	  "Paine",
	  "Barnard",
	  "Lloyd",
	  "Coffin",
	  "Taber",
	  "Dalton",
	  "Wise",
	  "Randolph",
	  "Hoyt",
	  "Shepard",
	  "Love",
	  "Street",
	  "Sanford",
	  "Short",
	  "Weeks",
	  "Wilder",
	  "Burt",
	  "Benson",
	  "Bean",
	  "Walter",
	  "Ackley",
	  "Baxter",
	  "Cleveland",
	  "Kirk",
	  "Knapp",
	  "Lake",
	  "Whitaker",
	  "Park",
	  "Forbes",
	  "Allison",
	  "Willard",
	  "Pike",
	  "Mckinney",
	  "Patton",
	  "Tracy",
	  "Stark",
	  "Decker",
	  "Kerr",
	  "Holbrook",
	  "Holden",
	  "Burke",
	  "Nicholson",
	  "Loomis",
	  "Hayden",
	  "Norman",
	  "Beach",
	  "Maynard",
	  "Chamberlain",
	  "Banks",
	  "Humphrey",
	  "Vaughn",
	  "Kellogg",
	  "Noble",
	  "Bassett",
	  "Fitch",
	  "Clifford",
	  "Roy",
	  "Conner",
	  "Cushing",
	  "Rich",
	  "Leach",
	  "Collier",
	  "Estes",
	  "Mortimer",
	  "Fry",
	  "Logan",
	  "Livingston",
	  "Bowers",
	  "Weber",
	  "Dutton",
	  "Beard",
	  "Huntington",
	  "Finley",
	  "Hatcher",
	  "Paul",
	  "Talbot",
	  "Cochran",
	  "Clay",
	  "Washburn",
	  "Wolf",
	  "Percy",
	  "Copeland",
	  "Lockwood",
	  "Wyatt",
	  "Elam",
	  "Vaughan",
	  "Griswold",
	  "Maxey",
	  "Brien",
	  "House",
	  "Vincent",
	  "Seymour",
	  "Hobbs",
	  "Schneider",
	  "Alden",
	  "Fields",
	  "Hatfield",
	  "Christian",
	  "Belcher",
	  "Kirby",
	  "Fish",
	  "Cameron",
	  "Fitzgerald",
	  "Massey",
	  "Stearns",
	  "Hayward",
	  "Richmond",
	  "Carson",
	  "Post",
	  "Hurst",
	  "Whipple",
	  "Wyman",
	  "Knox",
	  "Zimmerman",
	  "Tanner",
	  "Raymond",
	  "Shields",
	  "Newell",
	  "Hooper",
	  "Arthur",
	  "Whiting",
	  "Foote",
	  "Temple",
	  "Prince",
	  "Thomson",
	  "Hansen",
	  "Hay",
	  "Boggs",
	  "Littlefield",
	  "Olson",
	  "Hollingsworth",
	  "Ware",
	  "Starr",
	  "Pennington",
	  "Frazier",
	  "Lindsay",
	  "Dunham",
	  "Orr",
	  "Lyman",
	  "Lester",
	  "Hays",
	  "Emery",
	  "Merritt",
	  "Hood",
	  "Lacy",
	  "Hampton",
	  "Combs",
	  "Burnett",
	  "Byrd",
	  "Dunbar",
	  "Melton",
	  "Boyer",
	  "Poole",
	  "Patrick",
	  "Eddy",
	  "Wentworth",
	  "Noyes",
	  "Kemp",
	  "Prescott",
	  "Stratton",
	  "Duke",
	  "Stephenson",
	  "Hibbard",
	  "Davies",
	  "Seaman",
	  "Gifford",
	  "Leavitt",
	  "Carey",
	  "Cline",
	  "Becker",
	  "Walsh",
	  "Bull",
	  "Doty",
	  "Sheldon",
	  "Garrison",
	  "Hardin",
	  "Goodman",
	  "Mathews",
	  "Gay",
	  "Huffman",
	  "Connor",
	  "Mcclure",
	  "Sumner",
	  "Brock",
	  "Cotton",
	  "Bray",
	  "Curry",
	  "Greer",
	  "Mckenzie",
	  "Proctor",
	  "Mcguire",
	  "Anthony",
	  "Charles",
	  "Baird",
	  "Woodruff",
	  "Metcalf",
	  "Archer",
	  "Wilkins",
	  "Burr",
	  "Chapin",
	  "Hazen",
	  "English",
	  "Shoemaker",
	  "Bullock",
	  "Summers",
	  "Wolfe",
	  "Sr",
	  "Mayfield",
	  "Cannon",
	  "Mckee",
	  "Lyons",
	  "Bridges",
	  "Tabor",
	  "Gross",
	  "Sinclair",
	  "Churchill",
	  "Edward",
	  "Herbert",
	  "Ingram",
	  "Huff",
	  "Middleton",
	  "Hamrick",
	  "Hartman",
	  "Blackburn",
	  "Partridge",
	  "Atkins",
	  "Cheney",
	  "Locke",
	  "Dodson",
	  "Goss",
	  "Hathaway",
	  "Cutler",
	  "Blankenship",
	  "Crabtree",
	  "Stebbins",
	  "Barr",
	  "Kidd",
	  "Bentley",
	  "Potts",
	  "Atwood",
	  "Richard",
	  "Clapp",
	  "Waller",
	  "Holman",
	  "Ferrers",
	  "Rodgers",
	  "Sherwood",
	  "Ely",
	  "Mercer",
	  "Aldrich",
	  "Torrey",
	  "Galloway",
	  "Stokes",
	  "Compton",
	  "Flint",
	  "Cain",
	  "Fairbanks",
	  "Weston",
	  "Tripp",
	  "Morrow",
	  "Knowles",
	  "Ames",
	  "Mansfield",
	  "Mott",
	  "Bingham",
	  "Witt",
	  "Gardiner",
	  "Floyd",
	  "Grimes",
	  "Rush",
	  "Judd",
	  "Riggs",
	  "Hess",
	  "Peabody",
	  "Angleterre",
	  "Gregg",
	  "Strange",
	  "Fraser",
	  "Swan",
	  "Hensley",
	  "Furr",
	  "Nixon",
	  "Hebert",
	  "Mayo",
	  "Lancaster",
	  "Peckham",
	  "Everett",
	  "Key",
	  "Doyle",
	  "Barlow",
	  "Rowland",
	  "Frederick",
	  "Ashley",
	  "Eastman",
	  "Bradshaw",
	  "Hadley",
	  "Bullard",
	  "Conant",
	  "Houston",
	  "Prather",
	  "Dunlap",
	  "Larson",
	  "Small",
	  "Vance",
	  "Draper",
	  "Quinn",
	  "Head",
	  "Courtenay",
	  "Tate",
	  "Billings",
	  "Monroe",
	  "Beaumont",
	  "Hopper",
	  "Crow",
	  "Casey",
	  "Jensen",
	  "Burch",
	  "Pugh",
	  "Clements",
	  "Schultz",
	  "Downing",
	  "Lathrop",
	  "Yoder",
	  "Packard",
	  "Whitehead",
	  "Sloan",
	  "Sanborn",
	  "Herring",
	  "Fiske",
	  "Dickerson",
	  "Goodrich",
	  "Goddard",
	  "Moulton",
	  "Sampson",
	  "Swift",
	  "Sarah",
	  "Paddock",
	  "Hendricks",
	  "Cushman",
	  "Irwin",
	  "Latham",
	  "Holcomb",
	  "Riddle",
	  "North",
	  "Teague",
	  "Dickson",
	  "Johns",
	  "Terrell",
	  "Hitchcock",
	  "Dexter",
	  "Ayers",
	  "Jewett",
	  "Farley",
	  "Root",
	  "Leslie",
	  "Pickens",
	  "Hogan",
	  "Mathis",
	  "Frances",
	  "Stiles",
	  "Ewing",
	  "Holloway",
	  "Cantrell",
	  "Godfrey",
	  "Gage",
	  "Cooley",
	  "Blount",
	  "Scotland",
	  "Houghton",
	  "Ogden",
	  "Shaffer",
	  "Pease",
	  "Dubois",
	  "Kirkpatrick",
	  "Hagan",
	  "Workman",
	  "Mcbride",
	  "Wiley",
	  "Hilton",
	  "Stetson",
	  "Parrish",
	  "Rutledge",
	  "Cornell",
	  "Cary",
	  "Spiller",
	  "Ruggles",
	  "Thurston",
	  "Gilmore",
	  "Waterman",
	  "Moyer",
	  "Otis",
	  "Conrad",
	  "Penn",
	  "Dodd",
	  "Elder",
	  "Butcher",
	  "Hooker",
	  "Burnham",
	  "Hawley",
	  "Faulkner",
	  "Brady",
	  "Reese",
	  "Jarvis",
	  "Beckwith",
	  "Roach",
	  "Haworth",
	  "Chatfield",
	  "Best",
	  "Donaldson",
	  "Berkeley",
	  "Pace",
	  "Frank",
	  "Giles",
	  "Calvert",
	  "Flynn",
	  "Miner",
	  "Lovell",
	  "Benjamin",
	  "Glenn",
	  "Blackwell",
	  "Shattuck",
	  "Sweet",
	  "Crandall",
	  "Pitts",
	  "Beebe",
	  "Stoddard",
	  "Gillespie",
	  "Low",
	  "Goff",
	  "Malone",
	  "Macdonald",
	  "Drew",
	  "Minor",
	  "Denison",
	  "Shannon",
	  "Mackenzie",
	  "Mar",
	  "Phipps",
	  "Haggard",
	  "College",
	  "Carver",
	  "Trowbridge",
	  "Benton",
	  "Whitman",
	  "Bright",
	  "Plummer",
	  "Sheppard",
	  "Carlton",
	  "Leblanc",
	  "Aldridge",
	  "Horner",
	  "Ladd",
	  "Mack",
	  "Hussey",
	  "Hannah",
	  "Child",
	  "Soule",
	  "Boynton",
	  "Hills",
	  "Eliza",
	  "Durham",
	  "Pierson",
	  "Meyers",
	  "Hines",
	  "Elliot",
	  "Hobart",
	  "Hickman",
	  "Coe",
	  "Joseph",
	  "Polk",
	  "Greenwood",
	  "Sexton",
	  "Montague",
	  "Crocker",
	  "Meadows",
	  "Hutchins",
	  "Osgood",
	  "Betts",
	  "David",
	  "Carlson",
	  "Oct",
	  "Guthrie",
	  "Pelletier",
	  "Couch",
	  "Pollard",
	  "Mcgee",
	  "Rollins",
	  "Borden",
	  "Alley",
	  "Dye",
	  "Ellison",
	  "Woodbury",
	  "Fay",
	  "Earl",
	  "Mclaughlin",
	  "Braose",
	  "Esq",
	  "Finch",
	  "Sutherland",
	  "Tilley",
	  "Petty",
	  "Darling",
	  "Hough",
	  "Calhoun",
	  "Mccormick",
	  "Drury",
	  "Winn",
	  "Meeks",
	  "Tomlinson",
	  "Beasley",
	  "Pendleton",
	  "Cope",
	  "Correspondence",
	  "Law",
	  "Margaret",
	  "Bourne",
	  "Dow",
	  "Benedict",
	  "Corbin",
	  "Dec",
	  "Nickerson",
	  "Rader",
	  "Colby",
	  "Robert",
	  "Hanks",
	  "Webber",
	  "Christ",
	  "Wing",
	  "Denney",
	  "Winter",
	  "Dale",
	  "Aug",
	  "Good",
	  "Spooner",
	  "Spaulding",
	  "Garland",
	  "Klein",
	  "Fournier",
	  "Justice",
	  "Goad",
	  "Burleson",
	  "Rankin",
	  "Towne",
	  "Bernard",
	  "Ingalls",
	  "Nov",
	  "Landry",
	  "Dewey",
	  "Bolton",
	  "Waldron",
	  "Spence",
	  "Barron",
	  "Ratliff",
	  "Hartley",
	  "Cowan",
	  "Beal",
	  "Gleason",
	  "Wight",
	  "Mcintosh",
	  "Snider",
	  "Rutherford",
	  "River",
	  "Ellen",
	  "Davison",
	  "Jan",
	  "Pare",
	  "Comstock",
	  "Salisbury",
	  "Coffey",
	  "Sellers",
	  "Albert",
	  "Lang",
	  "Marvin",
	  "Gore",
	  "Vernon",
	  "Sanderson",
	  "Harlan",
	  "Basset",
	  "Ritchie",
	  "Neill",
	  "Spears",
	  "Titus",
	  "Buckley",
	  "Conley",
	  "Franks",
	  "Daugherty",
	  "Sept",
	  "Hodge",
	  "Clinton",
	  "Gooch",
	  "Fairchild",
	  "Treat",
	  "Fitzalan",
	  "Bohun",
	  "Whitcomb",
	  "Farrar",
	  "Gaines",
	  "Stover",
	  "Calkins",
	  "Jacob",
	  "Smyth",
	  "Hoskins",
	  "Lowry",
	  "Eldridge",
	  "Mcfarland",
	  "Dick",
	  "Downs",
	  "Shirley",
	  "Pickering",
	  "Drummond",
	  "Clement",
	  "Vere",
	  "Mclean",
	  "Haskell",
	  "Mcdowell",
	  "Gale",
	  "Story",
	  "Halstead",
	  "Springer",
	  "Carmichael",
	  "Barry",
	  "Clifton",
	  "Tompkins",
	  "Wadsworth",
	  "Glass",
	  "Queen",
	  "Herrick",
	  "Peirce",
	  "Gorham",
	  "Babb",
	  "Pettit",
	  "Cornett",
	  "Crouch",
	  "Feb",
	  "Craft",
	  "Frye",
	  "Masters",
	  "Mcconnell",
	  "Westfall",
	  "France",
	  "Gilman",
	  "Hewitt",
	  "Snell",
	  "Wallis",
	  "Mcmillan",
	  "Waite",
	  "Langston",
	  "Hanna",
	  "Flanders",
	  "Foley",
	  "Grove",
	  "Kline",
	  "Withers",
	  "Henson",
	  "Pool",
	  "Wills",
	  "Roe",
	  "Blevins",
	  "Ferris",
	  "Crockett",
	  "Rushing",
	  "Nye",
	  "Balch",
	  "Welles",
	  "Eller",
	  "Gibbons",
	  "Rowley",
	  "Brigham",
	  "Tilton",
	  "Swain",
	  "Connell",
	  "Crowell",
	  "Haley",
	  "Boyle",
	  "Standish",
	  "Corley",
	  "Funk",
	  "Ayres",
	  "Marks",
	  "Upton",
	  "Stinson",
	  "Gauthier",
	  "Hurd",
	  "Jefferson",
	  "Smart",
	  "Bauer",
	  "Slater",
	  "Piper",
	  "Mallory",
	  "Lindley",
	  "Stockton",
	  "Bragg",
	  "Harrell",
	  "Pittman",
	  "Cash",
	  "Pyle",
	  "Grace",
	  "Landers",
	  "Michael",
	  "Lehman",
	  "Golden",
	  "Louise",
	  "Griggs",
	  "Mccarty",
	  "Rector",
	  "Bancroft",
	  "Ingersoll",
	  "Wiggins",
	  "Waggoner",
	  "Valentine",
	  "Mckay",
	  "Burdick",
	  "Flowers",
	  "Scotia",
	  "Winters",
	  "Rider",
	  "Schwartz",
	  "Coffman",
	  "Peacock",
	  "Spalding",
	  "Langley",
	  "Douglass",
	  "Parke",
	  "Hungerford",
	  "Lawton",
	  "Mueller",
	  "Lowery",
	  "Grover",
	  "Tower",
	  "Brooke",
	  "Dillard",
	  "Thorne",
	  "Way",
	  "Tubbs",
	  "Ireland",
	  "Andrew",
	  "Skidmore",
	  "Wakefield",
	  "Gunn",
	  "Dorsey",
	  "Conway",
	  "Joyce",
	  "Louisa",
	  "Singleton",
	  "Fischer",
	  "Finney",
	  "Childs",
	  "Welsh"
	};
	public PPlayer (int newMoney, int newId, Color newColor)
	{
		Money = newMoney; 
		PlayerId = newId;
		BaseColor = newColor;
		Units = new List<GameObject>();
		EOTLevel = PConst.EOT_None;
		EOMTS = 0;	
	}

}
