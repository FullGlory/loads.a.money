Feature: Gather Price Data
	In order to be able to make mates
	As an investor
	I want access to current and historical stock price data
	
Scenario: Stock price is retrieved after a period of time
	Given a market exists
	And the market is open
	And the market contains stock
	And the price data capture routine is running
	When I check the content of the stock price database
	Then stock price data appears within 2 minutes
