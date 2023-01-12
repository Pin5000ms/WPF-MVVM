using ModernDesign.Core;
using ModernDesign.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernDesign.MVVM.ViewModel
{
    public class SudokuViewModel : ObservableObject
    {
        public SudokuViewModel()
        {
            CellList = new ObservableCollection<SudokuCell>();

            var CellVM1 = new SudokuCellVM(new List<string> { 
                "", "6", "",
                "2", "", "3",
                "", "", "" });
            CellList.Add(new SudokuCell(CellVM1));

            var CellVM2 = new SudokuCellVM(new List<string> {
                "", "2", "",
                "", "1", "5",
                "6", "", "3" });
            CellList.Add(new SudokuCell(CellVM2));

            var CellVM3 = new SudokuCellVM(new List<string> {
                "9", "1", "3",
                "6", "8", "",
                "2", "5", "4" });
            CellList.Add(new SudokuCell(CellVM3));


            var CellVM4 = new SudokuCellVM(new List<string> {
                "", "2", "",
                "1", "5", "",
                "", "", "" });
            CellList.Add(new SudokuCell(CellVM4));

            var CellVM5 = new SudokuCellVM(new List<string> {
                "", "", "1",
                "", "4", "",
                "2", "", "" });
            CellList.Add(new SudokuCell(CellVM5));

            var CellVM6 = new SudokuCellVM(new List<string> {
                "3", "", "",
                "", "", "6",
                "8", "9", "" });
            CellList.Add(new SudokuCell(CellVM6));



            var CellVM7 = new SudokuCellVM(new List<string> {
                "", "", "6",
                "4", "", "7",
                "9", "1", "2" });
            CellList.Add(new SudokuCell(CellVM7));

            var CellVM8 = new SudokuCellVM(new List<string> {
                "", "", "2",
                "", "9", "",
                "7", "", "" });
            CellList.Add(new SudokuCell(CellVM8));

            var CellVM9 = new SudokuCellVM(new List<string> {
                "", "7", "9",
                "", "6", "2",
                "5", "", "" });
            CellList.Add(new SudokuCell(CellVM9));
            Initial();
            Solve(Puzzle);
        }
        public ObservableCollection<SudokuCell> CellList { get; set; }

        private void Initial()
        {
            for (int i = 0; i < 9; i++)
            {
                var cell = CellList[i];
                for (int j = 0; j < 9; j++)
                {
                    var item = cell.CellVM.M[j];
                    int rowBase = i / 3;
                    int colBase = i % 3;
                    int row = j / 3;
                    int col = j % 3;
                    if (item == "")
                    {
                        Puzzle[rowBase * 3 + row, colBase * 3 + col] = 0;
                    }
                    else
                    {
                        Puzzle[rowBase * 3 + row, colBase * 3 + col] = int.Parse(item);
                    }
                }
            }
        }
        int[,] Puzzle = new int[9, 9];

        private bool Solve(int[,] Puzzle)
        {
            bool result = false;
            int row = -1;
            int col = -1;
            FindZeroRowCol(ref row, ref col);

            if (row == -1 && col == -1)
            {
                result = true;
                return result;
            }

            for (int k = 1; k < 10; k++)
            {
                if (CheckRowColCell(row, col, k))
                {
                    Puzzle[row, col] = k;
                    if (Solve(Puzzle))
                    {
                        result = true;
                        return result;
                    }
                    else
                    {
                        Puzzle[row, col] = 0;
                    }
                }

                //if (CheckRowColCell(row, col, k))
                //{
                //    Puzzle[row, col] = k;
                //    if (!Solve(Puzzle))
                //    {
                //        Puzzle[row, col] = 0;

                //        if (k == 9)
                //        {
                //            return false;
                //        }
                //    }
                //}
                //else if (k == 9)
                //{
                //    return false;
                //}
            }
            return result;
        }

        private bool CheckRowColCell(int row, int col, int input)
        {
            bool result = true;
            for (int idx = 0; idx < 9; idx++)
            {
                if (idx == col)
                    continue;
                if(Puzzle[row, idx] == input)
                {
                    return false;
                }
            }
            for (int idx = 0; idx < 9; idx++)
            {
                if (idx == row)
                    continue;
                if (Puzzle[idx, col] == input)
                {
                    return false;
                }
            }

            int rowBase = row / 3;
            int colBase = col / 3;
            rowBase *= 3;
            colBase *= 3;
            for (int i = rowBase; i < rowBase+3; i++)
            {
                for (int j = colBase; j < colBase + 3; j++)
                {
                    if (i == row && j == col)
                        continue;
                    if(Puzzle[i,j] == input)
                    {
                        return false;
                    }
                }
            }
            return result;
        }

        private void FindZeroRowCol(ref int row, ref int col)
        {
            for (int i = 0; i < 9; i++)//row
            {
                for (int j = 0; j < 9; j++)//col
                {
                    if (Puzzle[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        return;
                    }
                }
            }
        }
    }
}
