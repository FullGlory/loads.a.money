Feature: Loads a Money Price Gathering
	In order to be able to make mates
	As an investor
	I want access to current and historical stock price data

@smokeTest
@ignore
Scenario: Stock price is retrieved after a period of time
	Given that the price data service is running 
	When I check the content of the stock price database
	Then stock price data appears within 10 minutes
