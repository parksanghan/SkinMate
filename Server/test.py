import openai
import os
import json
from dotenv import load_dotenv

load_dotenv()
client = openai.OpenAI(api_key=os.getenv("OPENAI_API_KEY"))

# 진단 결과
diagnosis_result = {
    "class": {
        "forehead_wrinkle": 7,
        "frown_wrinkle": 1,
        "eyes_wrinkle": 1,
        "lips_dryness": 2,
        "jaw_sagging": 2,
        "cheek_pore": 0,
    },
    "regression": {
        "face": 0.1509,
        "forehead_moisture": 0.1054,
        "forehead_elasticity": 0.1551,
        "eyes_wrinkle": 0.0931,
        "cheek_moisture": 1.2114,
        "cheek_elasticity": 0.9109,
        "cheek_pore": 0.2971,
        "jaw_moisture": 0.0326,
        "jaw_elasticity": 0.0504,
    },
}

# 프롬프트 구성
prompt = f"다음 피부 진단 결과를 사람에게 설명해줘:\n```json\n{json.dumps(diagnosis_result, ensure_ascii=False, indent=2)}\n```"

# 최신 API 방식
response = client.chat.completions.create(
    model="gpt-4o",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": prompt},
    ],
)

# 응답 출력
print("💬 챗봇 응답:")
print(response.choices[0].message.content)
