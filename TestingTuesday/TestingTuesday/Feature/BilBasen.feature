@BilBasen
Feature: Search functionality in BilBasen
Check if the user is able to search for cars.


Scenario: Search for a vintage car
	Given User is in Bilbasen page
	When searches for car older than 2000
	Then bilbasen result private and dealer cars listed for the same

Scenario: Search car with user defined criteria
	Given User is in Bilbasen page
	When user search for car
	| Mærke		| km_Fra	| km_till		| År_fra |
	|  Audi     | 10.000 km |  100.000 km   |  2007	 |
	Then bilbasen result private and dealer cars listed for the same