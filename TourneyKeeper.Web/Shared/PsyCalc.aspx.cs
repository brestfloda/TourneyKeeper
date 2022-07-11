using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class PsyCalc : TKWebPage
    {
        private int[] values = new int[] { 1, 2, 3, 4, 5, 6 };

        protected void Page_Load(object sender, EventArgs e)
        {
            var numberOfDiceDropDownListContent = $"<option value='1'>1</option>";
            numberOfDiceDropDownListContent += $"<option value='2'>2</option>";
            numberOfDiceDropDownListContent += $"<option value='3'>3</option>";
            numberOfDiceDropDownListContent += $"<option value='4'>4</option>";
            numberOfDiceDropDownListContent += $"<option value='5'>5</option>";
            numberOfDiceDropDownListContent += $"<option value='6'>6</option>";
            numberOfDiceDropDownListContent += $"<option value='7'>7</option>";
            numberOfDiceDropDownListContent += $"<option value='8' selected>8</option>";
            numberOfDiceDropDownListContent += $"<option value='9'>9</option>";
            numberOfDiceDropDownListContent += $"<option value='10'>10</option>";
            numberOfDiceDropDownListContent += $"<option value='11'>11</option>";
            numberOfDiceDropDownListContent += $"<option value='12'>12</option>";
            numberOfDiceDropDownListLiteral.Text = numberOfDiceDropDownListContent;

            var minimumSuccessesDropDownListContent = $"<option value='1'>1</option>";
            minimumSuccessesDropDownListContent += $"<option value='2'>2</option>";
            minimumSuccessesDropDownListContent += $"<option value='3' selected>3</option>";
            minimumSuccessesDropDownListContent += $"<option value='4'>4</option>";
            minimumSuccessesDropDownListContent += $"<option value='5'>5</option>";
            minimumSuccessesDropDownListContent += $"<option value='6'>6</option>";
            minimumSuccessesDropDownListContent += $"<option value='7'>7</option>";
            minimumSuccessesDropDownListContent += $"<option value='8'>8</option>";
            minimumSuccessesDropDownListContent += $"<option value='9'>9</option>";
            minimumSuccessesDropDownListContent += $"<option value='10'>10</option>";
            minimumSuccessesDropDownListLiteral.Text = minimumSuccessesDropDownListContent;

            var minimumScoreDropDownListContent = $"<option value='1'>1</option>";
            minimumScoreDropDownListContent += $"<option value='2'>2</option>";
            minimumScoreDropDownListContent += $"<option value='3'>3</option>";
            minimumScoreDropDownListContent += $"<option value='4' selected>4</option>";
            minimumScoreDropDownListContent += $"<option value='5'>5</option>";
            minimumScoreDropDownListContent += $"<option value='6'>6</option>";
            minimumScoreDropDownListLiteral.Text = minimumScoreDropDownListContent;
        }

        protected void CalcClick(object sender, EventArgs e)
        {
            int numDice = int.Parse(numberOfDiceDropDownHiddenField.Value);
            float minimumScore = int.Parse(minimumScoreDropDownHiddenField.Value);
            int minimumSuccesses = int.Parse(minimumSuccessesDropDownHiddenField.Value);

            double prop = 0.0f;
            for (int i = minimumSuccesses; i < (numDice + 1); i++)
            {
                prop += Distribution.BinomialProbability(numDice, ((6.0 - minimumScore) + 1.0) / 6.0, i);
            }

            chanceOfSuccessTextBox.Text = prop.ToString();

            prop = 0.0f;
            for (int i = 2; i < (numDice + 1); i++)
            {
                prop += Distribution.BinomialProbability(numDice, 1.0 / 6.0, i);
            }

            chanceOfPerilsTextBox.Text = prop.ToString();
        }
    }
}