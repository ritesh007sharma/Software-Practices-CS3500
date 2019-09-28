Name: Ritesh Sharma
Sept 25th 2018: Read the instructions of PS4 to get a rough idea of what we should be 
				implementing in this assignment. Followed the instuctions given in the 
				assignment page throughly to set up the assignment, made a new resources
				project and linked formula dll and Dependency Dll to it. Building against the
				FormulaVersion.dll - 1.0.0.0, ForulaVersion.xml - 1.0 and DependencyGraphVersion
				.dll - 1.0.0.0 and DependencyGraphVersion.xml - 1.0.


Sept 26th 2018: Started by implementing a cell class that has a constuctor names cell which takes
				in string name as a parameter and setsContents and getsContent. Initialized a 
				Dependency graph and a dictionary with string as a key and Cell as a value. 
				Using the same dictionary looped through all the cell and got content of the name
				given to a GetcellContent name.

Sept 27th 2018: Initially my approach for setCellContent(double and string) was to make a temp 
				hashset with no elements and replace all dependents of stringname with empty 
				dependents and add that to a dictionary before returning all dependents. Then 
				I changed my aproach a little bit by making a hash set, replaceing dependents
				and make a new hashset again to replace dependents and return the final value.

				For the setting a formula, the only thing I changed was that formula now con
				tained dependents so I checked for formula dependents, created a new cell and 
				added new formula with all the dependents and returned using gets cells to re
				calculate.

Sept 28th, 2018: Planned on making a test case that would cover every single line of code that 
				was written by me, and was successfull on covering all code and got that
				100% test coverage.

Oct 2nd, 2018: The spreasheet class is built of a DependencyGraph, Formula Class and a Abstract SpreadSheet Class
				Went over PS5 specification and created PS5 branch, after creating a branch copied the
				abstractClass file from the repository and updated all the new methods for PS5.

Oct 4th, 2018: Wrote SetContentOftheCell method, had difficulty recalculating the values after setting
				the cell content and figured out that recalculating should be done in three methods 
				implemented in PS4 SetCellContent. Also implemented Save method majority of this method
				was implemented using the C# API for XML documentation, implemented GetSaved and also the 
				constructor for creating the path to the cell. Wrote some test cases, SetContentsOftheCell
				does not entirely work.

Oct 5th, 2018: Fixed SetContentsofTheCell wrote about 70 test cases, had difficulty writing the test 
				cases for getSavedVersion, finally with the help of TA wrote some test cases for
				getSavedVersion and passed all the test. The main difficulty for me in this assignment
				was implementing Save, getsavedVersion which I felt was challenging since we had to 
				deal with XML file.
