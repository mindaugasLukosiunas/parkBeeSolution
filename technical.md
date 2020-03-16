# parkBeeSolution

1. What testing frameworks are you using currently or have worked on recently?

Used NUnit, XUnit, Specflows (BDD), TestNG (in Java). Selenium as a tool for UI tests.

2. What do you think of them and would you want to use them again?

NUnit and XUnit are just popular frameworks for structuring tests in C# that
work good for Unit tests and also have parametrization options which is really great for API/Integration testing too.
I'm using SpecFlows (BDD) framework right now, but for UI testing I would stick to NUnit/Xunit.
BDD is really good to work on user stories and understanding the business logic and needs, but it greatly slows down test code development.

3. What future or current technology do you look forward to using the most and why?

Looking forward for more API testing, it's just something I have always wanted to do more, and tests run pretty fast and coud be developed quickly.
Also mocking some services would be great thing to learn/do.

4. How would you improve the dynamic pricing functionality or API methods you were testing (bug fixes, usability suggestions, etc.)?

What I noticed is that it takes a considerably long time to fully load the prices and the result page. This is something that's worth looking into.
I noticed that a page tries to load a lot of images that some take up to 3 seconds to show. Price loader also takes quite a while to calculate the price, maybe consider optimizing the price calculation.

Some considerations:

- Do all parking images need to be shown in the results page immediatly? I saw some images taking more than 3 seconds to laod.
Maybe could show the parking image only when a customer clicks on the parking spot - this woud reduce the load time of the page and would need to load only one parking spot image instead of all at once.

- Price calculation sometimes takes too long in the search result screen (from UI).

- Why does the price calculation endpoint use only one garageID? Why can't it post the list of garage IDs and return the prices in bulk?

- Noticed that it calculates the prices even if the dates posted are in the past. Not sure if that's needed, could be a validation when posting dates in the past.

- Some vlaidation for the dates in the future could also be there, since it does not properly calculate prices for ridiculous dates such as in the year 9999-01-01

5. How would you improve your test suite if you had more time?

- I did the test only for specific price calculation endpoint, which is dependant on the data from 2 other endpoints.
Currently I do hardcode the accessToken and garageID, but there should be methods for querying them dynamically.
- Also I did create a method for specificaly formatting the endpoint, but some kind of wrapper aound RestSharp client should be there too so that it could be more abstract and used for any other endpoint.
- Some different request Header types could also have been tested.
- Even though the endpoint method is POST, but the same endpint could be tested with GET, DELETE, UPDATE, etc method calls. Who knows what could happen ¯\_(ツ)_/¯
- More in depth Error message testing could be done.
- For UI tests I used a helper library that waits a bit too long, I could optimize the test for execution speed.
- UI tests selecting the date method could be improved to aways select the date in the future, since static data would fail at some point.
