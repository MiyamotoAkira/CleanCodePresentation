The following are the actions taken, plus commentaries on each one

## Visuals
1. get the right indentation all over the place
   Our brain uses pattern recognition to simplify the visual tasks.
2. eliminate all the unneded spacing

## Naming (1)
1. rename b to board
   This indicates what we are working with. We start to know about the domain we are working on.
2. rename i to row
   Now we assume the first one are the rows
3. rename j to column
   Then the columns.
4. create variable to hold the boardSize
   Eliminate magic number
5. replace all 8s for boardSize
   Magic numbers are bad. They don't have information about their meaning. Avoid themn.

6. rename x to rowToClear
7. rename y to columnToClear
8. rename k to columnToUpdate
9. rename l to columnToUpdate
	The above names are not very pretty, but they could be changed later on.

## Extract Methods (1)
1. extract external for into its own method -> ProcessRow
	Always eliminate levels of indentation.
2. extract method that creates result -> ExtractSolution
Because of the above, it is a bit more clear what is happening


## Keep levels of abstraction on a method coherent
1. now we can extract the for into a -> FindSolution
	We will need to make clear that this is easier to see now because of the previous extractions.
2. The first line is extracted -> CreateBoard
	The reason why is because now the Solver describes the conceptual actions that we have to take, and now how to take them. We stop mixing levels of abstraction
	Make the point that, so far, no objects have been created. Clean Code is about readability, not SOLID.

	
## Naming (2)
1. Create CellStatus enum
2. Change board from int[,] to -> CellStatus[,]
3. Change 0 when used on board value to -> CellStatus.Empty
4. Change 1 when used on board value to -> CellStatus.Occupied
5. Change 2 when used on board value to -> CellStatus.Threatened
   Now the values have a meaning
   You can see that we are leaving so far all duplication. when you eliminate duplication is not code that is exactly the same, but code that achieves that applies the same logic to the same elements. Single reason to change.
   ToDo: Need to show an example where it will not happen.

## Extract Methods (2)
1. extract the for columns inside Process -> ProcessCell
2. extract the if not found -> RevertLastQueenPlacement

## Naming (3)
1. rename found -> queenIsPlaced
   Inside the new process Cell we eliminate found completely.
   Need to replace break -> for return true
   Then outside of the if return false;
   The break has to happen after the call
2. rename ProcessRow -> TryPlaceQueenOnRow
   The name doesn't tell you anything about what is going on.
3. reanme ProcessCell -> TryPlaceQueenOnCell

## Extract Methods from conditions
1. Replace the if ->CellIsNotOccupied
   We want to clearly indicate what is the reason
2. Replace the if -> CellIsOccupied
3. Replace the if with Empty -> ICanPlaceQueen

## Consolidate arguments
1. Convert board, colum, row -> board[row, colum]
   They are used together, from the point of the method they are really a single entity
   Don't consolidate arguments just to reduce the number. Do they make sense consolidated?

## Extract Methods
1. Extract threatening to ->  MarkThreatenedCells
   Now we can do this becasuse not only the syntax is the same, the semantics of the code are exactly the same.
2. Extract internal method -> ThreatenSameRow
3. Extract internal method -> ThreatenSameColumn
4. Extract internal method -> ThreatenUpperLeftDiagonal
5. Extract internal method -> ThreatenLowerLeftDiagonal
6. Extract internal method -> ThreatenUpperRightDiagonal
7. Extract internal method -> ThreatenLowerRightDiagonal
8. Extract internal method -> ThreatenRightToLeftDiagonal
9. Extract internal method -> ThreatenLeftToRightDiagonal

