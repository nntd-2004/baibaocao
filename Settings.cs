namespace SnakeGames
{
    // Enum để định nghĩa các hướng di chuyển của rắn trong game
    public enum Directions
    {
        Left,   // Di chuyển sang trái
        Right,  // Di chuyển sang phải
        Up,     // Di chuyển lên trên
        Down    // Di chuyển xuống dưới
    };

    // Lớp Settings chứa các cài đặt và thuộc tính của game
    public class Settings
    {
        // Các thuộc tính cài đặt của game
        public static int Width { get; set; } // Chiều rộng của một ô (tính theo pixel hoặc đơn vị)
        public static int Height { get; set; } // Chiều cao của một ô
        public static int Speed { get; set; } // Tốc độ di chuyển của rắn
        public static int Score { get; set; } // Điểm số hiện tại
        public static int Points { get; set; } // Điểm nhận được khi ăn thức ăn
        public static bool GameOver { get; set; } // Trạng thái game (kết thúc hay chưa)
        public static Directions direction { get; set; } // Hướng di chuyển hiện tại của rắn

        // Hàm khởi tạo (constructor) mặc định của lớp Settings
        public Settings()
        {
            Width = 16;           // Đặt chiều rộng mặc định của mỗi ô là 16
            Height = 16;          // Đặt chiều cao mặc định của mỗi ô là 16
            Speed = 10;           // Đặt tốc độ mặc định của rắn là 20
            Score = 0;            // Điểm số bắt đầu là 0
            Points = 100;         // Mỗi lần ăn thức ăn, cộng thêm 100 điểm
            GameOver = false;     // Trạng thái bắt đầu của game là chưa kết thúc
            direction = Directions.Down; // Hướng di chuyển mặc định là đi xuống
        }
    }
}
