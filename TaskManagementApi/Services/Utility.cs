// ユーティリティクラス
namespace TaskManagementApi.Services
{
    public static class Utility
    {
        // UTC日付を取得
        public static DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        // null安全な文字列トリム
        public static string? SafeTrim(string? input)
        {
            return input?.Trim();
        }

        public static void FizzBuzz()
        {
            // FizzBuzzの実装はここに追加
            // 例: 1から100までの数を出力し、3の倍数ならFizz、 5の倍数ならBuzz、両方ならFizzBuzzを出力
            for (int i = 1; i <= 100; i++)
            {
                if (i % 15 == 0)
                    Console.WriteLine("FizzBuzz");
                else if (i % 3 == 0)
                    Console.WriteLine("Fizz");
                else if (i % 5 == 0)
                    Console.WriteLine("Buzz");
                else
                    Console.WriteLine(i);
            }
        }
    }
}