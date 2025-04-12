from fastapi import FastAPI, HTTPException, UploadFile, File
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from db.db_manager import DbManager
import os
from models import RegisterRequest, LoginRequest
import uvicorn
import traceback
from datetime import datetime
from chat.chat_manager import ChatManager
from fastapi import Form

chat_manager = ChatManager()
app = FastAPI()
# CORS 허용
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # 나중에 실제 도메인으로 제한 가능
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)
# Supabase 환경 변수
SUPABASE_URL = os.getenv("SUPABASE_URL")
SUPABASE_KEY = os.getenv("SUPABASE_KEY")
# DB 매니저 초기화
db_manager = DbManager(SUPABASE_URL, SUPABASE_KEY)


@app.get("/")
def read_root():
    return {"message": "Hello FastAPI!"}


# 로그인 API
@app.post("/login")
def login(req: LoginRequest):
    success = db_manager.login_user(req.UserId, req.Password)
    if success:
        return {"status": "ok"}
    raise HTTPException(status_code=401, detail="invalid")


# 회원가입 API
@app.post("/register")
def register(req: RegisterRequest):
    try:
        db_manager.register_user(req.UserId, req.Password)
        return {"status": "ok"}
    except Exception as e:
        # 예외 발생 시 상세한 로그 기록
        error_message = f"Error occurred: {str(e)}"
        traceback.print_exc()
        raise HTTPException(status_code=500, detail=error_message)


# 파일 업로드 API
@app.post("/{user_id}/upload")
async def upload(
    user_id, files: list[UploadFile] = File(...)
):  # 👈 여기 'files'로 바뀐 것 주의
    try:
        os.makedirs("uploads", exist_ok=True)
        for file in files:
            path = os.path.join("uploads", file.filename)
            with open(path, "wb") as f:
                f.write(await file.read())
        return {"status": "ok"}
    except Exception as e:
        print(f"❌ 업로드 실패: {e}")
        raise HTTPException(status_code=500, detail="fail")


@app.get("/{user_id}/logs")
async def request_user_logs(user_id: str):
    try:
        logs = db_manager.get_user_logs(user_id)
        return logs
    except Exception as e:
        print(f"❌ 요청 실패: {e}")
        raise HTTPException(status_code=500, detail="fail")


logs = [
    {
        "chat_id": 1,
        "user_id": 1,
        "log_type": "질의응답",
        "image_path": None,
        "diagnosis_result": None,
        "message": "주름 개선 방법이 뭐야?",
        "response": "주름 개선에는 수분크림이 효과적입니다.",
        "timestamp": datetime(2025, 3, 31, 21, 55, 11),
    },
    {
        "chat_id": 2,
        "user_id": 1,
        "log_type": "진단분석",
        "image_path": "/path/to/image.jpg",
        "diagnosis_result": '{"jawline": 4, "wrinkle": 2}',
        "message": None,
        "response": "주름은 평균보다 많고 턱선은 준수합니다.",
        "timestamp": datetime(2025, 3, 31, 21, 55, 11),
    },
]


@app.post("/{user_id}/chat")
async def request_chat(user_id, message: str = Form(...)):
    logs = db_manager.get_user_logs("user_id")
    user_message = message
    response = chat_manager.request_chat_response(logs, user_message)
    return {"status": "ok", "msg": response}


if __name__ == "__main__":
    uvicorn.run("app:app", host="0.0.0.0", port=8080, reload=True)
