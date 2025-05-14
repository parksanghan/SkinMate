import os, mimetypes, asyncio
from db.db_manager import DbManager  # DbManager 경로에 맞게 조정

SUPABASE_URL = os.getenv("SUPABASE_URL")
SUPABASE_KEY = os.getenv("SUPABASE_KEY")

db_manager = DbManager(SUPABASE_URL, SUPABASE_KEY)

username = "1"  # 이미 존재하는 계정
file_path = r"C:\Users\박상한\Desktop\captured_20250513_203011.jpg"
filename = os.path.basename(file_path)

with open(file_path, "rb") as f:
    data = f.read()

mime_type = mimetypes.guess_type(file_path)[0] or "application/octet-stream"


async def main():
    url = await db_manager.upload_image_to_storage(username, filename, data, mime_type)
    print(f"[RESULT] 이미지가 업로드된 URL → {url}")


asyncio.run(main())
