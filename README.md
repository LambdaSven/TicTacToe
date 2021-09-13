
# TicTacToe

This was the take-home final exam for High Quality Software Programming at Conestoga College. This project was implemented from start to finish in a matter of 3 days.

![tic-tac-toe](https://user-images.githubusercontent.com/50975120/132905493-2b7bfebb-8102-4f6f-b259-e2cc8a534649.png)

This is a GUI-Based Tic Tac Toe game, with a full AI and save/load functionality.

The AI generates the game tree from a given position, and uses Minimax to determine the optimal move. 

The GUI is does using the MVVM model, with caliburn micro as a library to facilite convienent data binding.

The Saves are XML files, as was specified by the exam format, and are saved in a folder in the same directory as the project. If I were to make this again I would use AppData
But the specification of the exam was clear.

The Testing Suite only covers functionality of the Save/Load functionality, as was specified explicitly on the specifications of the exam.
