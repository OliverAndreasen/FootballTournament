# Football Tournament


## Introduction

This is a simple application that reads a set of files containing football matches and outputs the results of the tournament.
Functionality for creating and managing leagues, teams, and matches, as well as calculating standings based on match results.


## Setup
To use your own data, you can create your own files in the `Files` folder.


**The setup.csv contains the Leagues that the rounds can be played in and contains the following information:**

```
SuperLigaen,1,2,3,0,2
NordicBetLigaen,0,1,0,2,2
```

The first column is the name of the league.

The second column is number of positions to promote to Champions league.

the third column is the number of subsequent positions to promote to Europe league, 

the fourth column is the number of subsequent positions to promote to Conference League

the fifth column is the number of positions to promote to an upper league

At last the sixth column is the number of final positions that are to be relegated into a lower league (usually 2)

**The Teams file has all the teams following information:**

``` 
Abbreviation,Full club name,Special ranking

AAB,Aalborg Boldspilklub,W
BIF,Brøndby IF,0
FCN,FC Nordsjælland,P
FCK,FC København,0
AB,Akademisk Boldklub,R
FCM,FC Midtjylland,0
ACH,AC Horsens,0
AGF,AGF Aarhus,0
OB,OB Odense,0
SIF,Silkeborg IF,R
SJE,SønderjyskE,0
VFF,Viborg FF,P   

```
(W-last years champion, C-Last years cup winner, P-Promoted team, R-Relegated team, 0 No special ranking)


The Rounds folder needs to have round files with all the teams playing in that round.

The format of the round file is as follows:

```
AAB,VFF,5-2
BIF,SJE,3-3
FCN,SIF,5-1
FCK,OB,4-2
AB,AGF,4-5
FCM,ACH,4-4
```

The first column is the home team.

The second column is the away team.

THe third column is the score of the match.

This format is used for all rounds but after the first 22 rounds the format is as follows:

``` 
FCK,BIF,5-1
FCM,AGF,5-2
FCN,ACH,1-3
AAB,AB,3-5
SIF,VFF,0-4
OB,SJE,1-0
```

The first 3 Matches are upper bracket matches and the last 3 are lower bracket matches.


## Testing

The application includes several test cases in the `Files/Test` folder for testing different scenarios:

1. Team plays twice in the same round - `TeamPlaysTwiceInSameRound` folder.
2. Team plays twice as home team against the same team in the initial 22 rounds - `TeamPlaysTwiceInitialRounds` folder.
3. Unknown team in the round file is not processed - `UnknownTeamNotProcessed` folder.
4. A team is not allowed to play against itself - `TeamPlayingAgainstItself` folder.

In the program.cs you can uncomment any of the test cases to run them.

### Here is some general rules that must be followed: 

1. Only teams known from teams file should be processed (even though other results may be in there)

2. During the first 22 rounds, you can only play against the same team in one home and one away match. After 22 rounds, the same applies again, but this time for teams inside each fraction

3. In Denmark you are not allowed to play against yourself

4. If games had to be cancelled and postponed, they would reside in a file called round-x-a.csv, where the a represents an incremental additional number.

5. For the initial rounds, only a league table is shown. After the split, two tables of the upper and lower fraction must be presented separately
Any custom rules you deem necessary, which are then explained
