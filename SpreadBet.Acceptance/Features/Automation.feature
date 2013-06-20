Feature: Loads Of Money Automation
	In order to become extremely rich, trades need to be triggered automatically

Scenario: 1.  Automatically open trades at bullbearings
	Given the valid account
	| Username              | Password |
	| jpcgoodby@yahoo.co.uk | trigger  | 
	And the following bets
	| Code | Name                   | Amount | Type |
	| LLOY | Lloyds Bank            | 100    | BUY  |
	| RBS  | Royal Bank Of Scotland | 45     | SELL |
	Then open the following bets
	| Code | Name                   | Amount | Type |
	| LLOY | Lloyds Bank            | 100    | BUY  |
	| RBS  | Royal Bank Of Scotland | 45     | SELL |

Scenario: 2.  Automatically close trades at bullbearings
	Given the valid account
	| Username  | Password |
	| jpcgoodby | trigger  | 
	And the following bets
	| Code | Name                   | Amount | Type |
	| LLOY | Lloyds Bank            | 100    | BUY  |
	| RBS  | Royal Bank Of Scotland | 45     | SELL |
	Then close the following bets
	| Code | Name                   | Amount | Type |
	| LLOY | Lloyds Bank            | 100    | BUY  |
	| RBS  | Royal Bank Of Scotland | 45     | SELL |

Scenario: 3.  Verify trade latest prices
	Given the valid account
	| Username  | Password |
	| jpcgoodby | trigger  | 
	And the following stocks
	| Code | Name                   | Bid | Offer | Security |
	| LLOY | Lloyds Bank            | 60  | 62    | FTSE     |
	| RBS  | Royal Bank Of Scotland | 300 | 302   | FTSE     |
	Then following stocks price should be updated
	| Code | Name                   | Bid | Offer |
	| LLOY | Lloyds Bank            | 60  | 62    | 
	| RBS  | Royal Bank Of Scotland | 300 | 302   |