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
import requests

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


@app.post("/{user_id}/upload")
async def upload(user_id: str, files: list[UploadFile] = File(...)):
    try:
        # 실제 파일 읽기는 유지 (파일 업로드 구조 유지 목적)
        for file in files:
            _ = await file.read()  # 파일 읽기만 하고 사용은 안함

            # Mock 응답 데이터
            fake_response = {
                "status": "ok",
                "msg": "진단 서버 응답 출력 완료",
                "diagnosis_result": {
                    "class": {
                        "forehead_wrinkle": 7,
                        "frown_wrinkle": 1,
                        "eyes_wrinkle": 1,
                        "lips_dryness": 2,
                        "jaw_sagging": 2,
                        "cheek_pore": 0,
                    },
                    "regression": {
                        "face": 0.1509101390838623,
                        "forehead_moisture": 0.1054084300994873,
                        "forehead_elasticity": 0.1551273763179779,
                        "eyes_wrinkle": 0.09314807504415512,
                        "cheek_moisture": 1.2114288806915283,
                        "cheek_elasticity": 0.9109305143356323,
                        "cheek_pore": 0.29711878299713135,
                        "jaw_moisture": 0.03263622894883156,
                        "jaw_elasticity": 0.050414279103279114,
                    },
                },
            }

            print("[✅ MOCK 추론 서버 응답]")
            print(fake_response["diagnosis_result"])
            return fake_response

        return {"status": "fail", "msg": "No files processed"}

    except Exception as e:
        print(f"❌ 업로드 실패: {e}")
        raise HTTPException(status_code=500, detail=str(e))


## 아래가 이전사용
# @app.post("/{user_id}/upload")
# async def upload(user_id: str, files: list[UploadFile] = File(...)):
#     try:
#         for file in files:
#             # 1. 이미지 내용 읽기
#             contents = await file.read()

#             # 2. 외부 진단 서버 주소
#             diagnosis_url = "http://182.210.98.131:5000/diagnose"

#             # 3. forwarding 요청
#             response = requests.post(
#                 diagnosis_url,
#                 files={"image": (file.filename, contents, file.content_type)},
#             )

#             # 4. 응답 확인 (예시: 첫 번째 응답만 반환)
#             if response.status_code != 200:
#                 raise Exception(
#                     f"추론 서버 오류: {response.status_code} - {response.text}"
#                 )
#             print("[✅ 추론 서버 응답]")
#             print(response.json())
#             return {
#                 "status": "ok",
#                 "msg": "진단 서버 응답 출력 완료",
#                 "diagnosis_result": response.json(),
#             }
#             # return {
#             #     "status": "ok",
#             #     "diagnosis_result": response.json(),  # 진단 결과 그대로 반환
#             # }

#         return {"status": "fail", "msg": "No files processed"}
#     except Exception as e:
#         print(f"❌ 업로드 실패: {e}")
#         raise HTTPException(status_code=500, detail=str(e))


# # 파일 업로드 API
# @app.post("/{user_id}/upload")
# async def upload(
#     user_id, files: list[UploadFile] = File(...)
# ):  # 👈 여기 'files'로 바뀐 것 주의
#     try:
#         os.makedirs("uploads", exist_ok=True)
#         for file in files:
#             path = os.path.join("uploads", file.filename)
#             with open(path, "wb") as f:
#                 f.write(await file.read())
#         return {"status": "ok"}
#     except Exception as e:
#         print(f"❌ 업로드 실패: {e}")
#         raise HTTPException(status_code=500, detail="fail")


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


@app.get("/getlog")
async def request_log():
    return logs


@app.post("/{user_id}/chat")
async def request_chat(user_id, message: str = Form(...)):
    logs = db_manager.get_user_logs(user_id)
    user_message = message
    response = chat_manager.request_chat_response(logs, user_message)
    return {"status": "ok", "msg": response}


if __name__ == "__main__":
    uvicorn.run("app:app", host="0.0.0.0", port=8080, reload=True)
