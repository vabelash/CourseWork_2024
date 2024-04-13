using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace exam_b
{
    public class Game
    {
        public static int exerciseIndex { get; set; }
        public static int gameTime = 0;
        public static int currentScore = 0;
        public static int carSpeed = 5;

        #region Помеха справа
        /// <summary>
        /// Упражнение "помеха справа": движение серой машины вперед
        /// </summary>
        /// <param name="carAI"></param>
        /// <param name="pictureBoxRoadLeft"></param>
        public static void GoRightCar(PictureBox carAI, PictureBox pictureBoxRoadLeft)
        {
            if (carAI.Left != pictureBoxRoadLeft.Right)
            {
                carAI.Left -= carSpeed;
            }
        }

        /// <summary>
        /// Движение красной машины вперед
        /// </summary>
        /// <param name="carAI"></param>
        /// <param name="pictureBoxRoadLeft"></param>
        public static void GoPlayer(PictureBox player, PictureBox pictureBoxRoadUp)
        {
            if (player.Bottom != pictureBoxRoadUp.Top)
            {
                player.Top -= carSpeed;
            }
        }

        /// <summary>
        /// Проверка машины, проехавшей первой
        /// </summary>
        /// <param name="player"></param>
        /// <param name="carAI"></param>
        /// <returns></returns>
        public static bool CheckFirst(PictureBox player, PictureBox carAI, Panel zone)
        {
            if (player.Top <= zone.Bottom) // если игрок заехал в зону
            {
                if (carAI.Right <= zone.Left) // если серый ее уже проехал
                {
                    // все хорошо
                    return true;
                }
                else if (carAI.Left >= zone.Right) // если серый еще не заехал
                {
                    // не пропустил помеху справа
                    return false;
                }
                else // серый тоже в зоне
                {
                    
                    return false;
                }
            }
            else // игрок еще не заехал в зону
            {
                return true;
            }
        }
        #endregion

        #region Красный сигнал

        public static bool CheckSignal(PictureBox player, PictureBox zone)
        {
            if (player.Top <= zone.Bottom - player.Height) // если игрок проехал перекресток
            {
                return true;
            }
            else // если игрок стоит
            {
                return false;
            }
        }
        #endregion

    }
}
