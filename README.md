Applications
============

Loads-a-Money Stock Service
---------------------------
This application will use web scraping technology to retrieve stocks and their prices from an online site. There are options to execute the application either as a console or a windows service.

Loads-a-Money Bet Decision Service
----------------------------------
Perfom a GET request against this REST service to get a list of all possible stocks to place bets on, along with information about the position, bet amount and the exit price.

Key parameters include:
> **MaxBet** - This is the maximum per point bet

> **MaxLoss** - This is the maximum amount of money we are willing to loose

> **SpreadLossRatio** (Optional) - this is the percentage of the MaxLoss amount that can be because of the spread.

Note: The difference between the bid and offer prices of a stock represents the spread. When betting that the price of stock will increase we effectively "buy" in at the bid price and have to wait for the price of the stock to increase to a point where the offer price = the original bid price before we start to make any money.

Similarly if we bet that a stock will reduce in price we effectively "buy" in at the offer price, and have to wait for the bid price to reduce to the original offer price before we start to make any money.

The "Mid" price of the stock is the price half way between the bid and the offer price. 

The size of the bet will depend on the amount of money we are willing to loose due to the spread. The larger the spread the smaller the bet amount.

**Example:**  
If we are willing to bid up to £100.00 per point, and decide on a spreadLossRatio of 50% this means we are willing to loose up to £50 immediately due to the stocks spread. If the spread of the stock is 5 points (say OFFER:**100.5** BID:**105.5**) then the maximum we can afford to bet is **£10.00** per point. If the price of the stock were to increase by 30 points during the course of the next 24 hours then the OFFER price will be **130.5**. This means we will have won **30 * £10.00 = £300.00**.

So what if the position moves against us? Well, we have stated that the maximum we are willing to loose is £100.00 and we have established that we are already going to be loosing £50.00 due to the spread and that our bid amount is £10.00. This means that we can afford for the Offer price to reduce by another 5 points. That means that our **EXIT POSITION** is where the offer price becomes **95.5**. At this point we will have lost **£100.00**

Loads-a-Money Stock Service
---------------------------
This REST service will enable us to query our current portfolio, query and save stock prices, and place bets by automating the online spreadBetting service.

Current Online Services
-----------------------
>**BullBearings** - Provides the ability to place bets
>**LiveChars.co.uk** - Online site containing stocks and prices. 