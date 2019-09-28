Name: Ritesh Sharma, Ajay Bagali

October 15th 2018:
					We met up on the Monday after fall break. We read through the assignment multiple times. We figured out whoes repository we
					were going to use and started working from their repository. We fixed Ritesh's PS5 code from the tests that were failing from the 
					PS5 grading tests. We had difficulty figuring out the set up of the assignment because the merging back to the master branch part
					was pretty confusing. Another difficult part was the intial setup for the skeleton and the demo classes. We were having some configuration
					problems, but once we asked the TA for the setup we were finished with the total setup. 
					Time: 2-3 hours

October 16th 2018:
					We started thinking about design and how were going to implement things in a general way. We worked around excel's spreadsheet and figured
					some of the nitty bitty things. This way we could actually understand the differences and its overall functionality. We also looked at one 
					of the previous labs to understand how the visual studio GUI works as well. We looked at the assignment specs and we sort of planned out the different
					features we were going to implement. We also looked into the methods that the spreadsheet panel class had so we could use them to our advantage. We
					understood how the spreadhsheet was laid out and how the panel class and the spreadsheet class worked with eachother.
					Time: 2-3 hours 

October 17th 2018: 
					We met up, but since we had the 3500 exam the next day we studied for the exam instead. 

October 18th 2018: 
					We met up after the exam. This is when we started to implement our code into the spreadsheet. For most of the specs and the little features
					we looked at how excel did it and we simply mirrored them. We added multiple text boxes that shows the names and the contents of the cell. This took
					a while updating and being able to enter into the text box. We also had problems where we didn't want to press enter each time a cell existed or was
					evaluated. Once we fixed this by finishing the selection changed method, so we could simlpy just select another cell and it would be evaluated
					rather than having to press enter each time. We also used the getSelection method to retreive on the row and the column. To be able to put certain values
					back into certain cells, we had to make a helper method called ascii converter. This method basically takes in the row and the column and converts it
					to the cell name displayed on the spreadsheet. 
					Time: 3-4 hours
					
October 19th 2018: 
					We met up and started working on the overall functionality of the spreadsheet. We added in arrow keys to navigate around the cells. This was 
					difficult because we had to work around the even handler from the spreadhseet panel class. We still have a problem with the scroll bar but that
					isn't really a top priority. We added in a few more features such as being able to save without clicking the enter button. We were able to do this
					in the selection changed. We also handled on how to be able to view the contents and the value at once using the text boxes above the spreadsheet. 
					After we had this functionality down, we worked on the file,save,open, and the close functions. This took some time to understand because we were
					watching Kopta's lectures.
					Time: 5-6 hours
								
October 22nd 2018: 
					
					We met up and we started right away on a few of our bugs. We had a problem where we werent deleting the cell's contents properly after we would
					move away from it. Once we figured how to clear the cells contents by using the empty string, we started to work on the clear button. This took some time
					because we were trying to understand how to clear the cells in the backend spreadhseet and in the spreadsheet panel class. Once we figured that out
					we started cleaning everything up and started to comment. We made a few helper methods and we also made sure the functionality of the spreadsheet was 
					spot on. 

					Time: 5-6 hours

 Features:          1.	Our spreadsheet allows the user to navigate using the arrow keys on their keyboard. Another additonal feature that these arrow keys do is that
						once the user leaves the selected cell,it evaluates the contents of that cell. 


					2.  In the File menu of our spreadsheet, we added in an additional option that clears cells of the current spreadsheet. It also asks the user to save any
						work that existed in that current spreadsheet. 

					3.	If the contents of that cell is a formula, we make the color of the formula in blue.

					4.  Our spreadsheet allows copy and pasting contents of the cell without selecting the value. For copy and paste, press Ctrl + c 
					    in the cell you want to copy dont worry about selecting the value and Ctrl + B, this will copy and paste the value 
						without selecting the content.

Special Insructions: 1.	In our spreadsheet, the user doesn't have to press enter each time for the contents to be stored/calculated. The user can direcectly go to 
						another cell by either selecting a new cell or by using the arrow keys. 

					 2. The user can also type the contents of the selected cell in the contents box right underneath the menu bar.
					 
					 3. The user can Copy/Paste using Ctrl + c(copy) and Ctrl + B(paste) without worrying about selecting the content.