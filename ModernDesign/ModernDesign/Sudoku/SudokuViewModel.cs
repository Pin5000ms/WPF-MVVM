using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernDesign
{
    public class SudokuViewModel : ObservableObject
    {
        public SudokuViewModel()
        {
            WaitTime = "20";
            CellList = new ObservableCollection<SudokuCell>();

            SolveCommand = new RelayCommand(   
            o => {
                InitialPuzzle();
                Solve(Puzzle);
                });

            ResetCommand = new RelayCommand(
                o => {
                    Question1();
                    InitialPuzzle();
                    Update();
                });
            ClearCommand = new RelayCommand(
                o => {
                    Clear();
                });

            Question1();
            InitialPuzzle();
        }
        public ObservableCollection<SudokuCell> CellList { get; set; }
        public RelayCommand SolveCommand { get; set; }

        public RelayCommand ResetCommand { get; set; }

        public RelayCommand ClearCommand { get; set; }

        private void Question1()
        {
            CellList.Clear();
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
        }

        private void InitialPuzzle()
        {
            for (int i = 0; i < 9; i++)
            {
                var cell = CellList[i];
                for (int j = 0; j < 9; j++)
                {
                    var item = cell.CellVM.CellItem[j].Number;
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

        private void Update()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int rowBase = i / 3;
                    int colBase = j / 3;

                    int num = rowBase * 3 + colBase;
                    var cell = CellList[num];

                    int retRow = i - rowBase * 3;
                    int retCol = j - colBase * 3;
                    if(Puzzle[i, j] != 0)
                    {
                        cell.CellVM.CellItem[retRow * 3 + retCol].Number = Puzzle[i, j].ToString();
                    }
                    else
                    {
                        cell.CellVM.CellItem[retRow * 3 + retCol].Number = "";
                    }
                    
                }
            }
        }

        private void Clear()
        {
            CellList.Clear();
            for (int i = 0; i < 9; i++)
            {
                CellList.Add(new SudokuCell(new SudokuCellVM(new List<string> {
                "", "", "",
                "", "", "",
                "", "", "" })));
            }
        }
        private void UpdateOneCell(int i, int j, Color c)
        {
            int rowBase = i / 3;
            int colBase = j / 3;

            int num = rowBase * 3 + colBase;
            var cell = CellList[num];

            int retRow = i - rowBase * 3;
            int retCol = j - colBase * 3;
            cell.CellVM.CellItem[retRow * 3 + retCol].Number = Puzzle[i, j].ToString();
            cell.CellVM.CellItem[retRow * 3 + retCol].BorderBrush.Color = c;
            WaitNMilliSeconds(int.Parse(WaitTime));
        }

        int[,] Puzzle = new int[9, 9];


        public string WaitTime { get; set; }
        private void WaitNMilliSeconds(int segundos)
        {
            if (segundos < 1) return;
            DateTime _desired = DateTime.Now.AddMilliseconds(segundos);
            while (DateTime.Now < _desired)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }
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
                    UpdateOneCell(row, col, Colors.LightGreen);
                    if (Solve(Puzzle))
                    {
                        result = true;
                        return result;
                    }
                    else
                    {
                        Puzzle[row, col] = 0;
                        UpdateOneCell(row, col, Colors.Red);
                    }

                }
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
