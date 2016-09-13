@Umbraco
Feature: Rendering TextPage
	In order to render an Umbraco textpage
	As a developer
	I want to be able to call .Render and get rendered HTML

Scenario: Render learn/basics
	When I render /learn/basics
	Then the result should contain "Learn the basics"
