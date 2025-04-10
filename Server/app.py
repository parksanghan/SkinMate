from fastapi import FastAPI, HTTPException, UploadFile, File
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from db.db_manager import DbManager
import os
from models import RegisterRequest, LoginRequest
import uvicorn
import traceback

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
@app.post("/upload")
async def upload(files: list[UploadFile] = File(...)):  # 👈 여기 'files'로 바뀐 것 주의
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


@app.post("/chat")
async def request_chat(message: str):
    return


if __name__ == "__main__":
    uvicorn.run("app:app", host="0.0.0.0", port=8080, reload=True)
