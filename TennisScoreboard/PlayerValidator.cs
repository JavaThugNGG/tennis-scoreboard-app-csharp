using System.Text.RegularExpressions;

namespace TennisScoreboard
{
    public class PlayerValidator
    {
        private static readonly int MaxPlayerNameLength = 16;

        public void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Имя игрока не может быть пустым.");
            }

            if (name.Length > MaxPlayerNameLength)
            {
                throw new IllegalPlayerNameException("Имя игрока слишком длинное. Максимум " + MaxPlayerNameLength + " символов.");
            }

            if (!Regex.IsMatch(name, @"^[a-zA-Zа-яА-ЯёЁ ]+$"))
            {
                throw new IllegalPlayerNameException("Имя игрока содержит недопустимые символы.");
            }
        }

        public void ValidateId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Некорректное значение id игрока: " + id);
            }

            if (!Regex.IsMatch(id, @"[a-zA-Zа-яА-ЯёЁ\- ]+"))
            {
                throw new IllegalPlayerNameFilterException("Фильтр содержит недопустимые символы");
            }
        }
    }
}
