@Umbraco
Feature: Posting to actions
	In order to avoid silly mistakes
	As a programmer idiot
	I want to be told my code can add two numbers

Scenario: Add two numbers
	Given I have entered 50 into A
	And I have entered 70 into B
	When I press add
	Then the result should contain 
    """
        <label name="result">120</label>
    """
