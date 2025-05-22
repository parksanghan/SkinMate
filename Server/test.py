# app.py

from fastapi import FastAPI

app = FastAPI()


@app.get("")
async def read_root():
    return {"message": "Hello, 박상한 병장님!"}


@app.get("/rr")
async def rr():
    return {"message": "Hello, 박상한 병장sdsad님!"}


if __name__ == "__main__":
    import uvicorn

    uvicorn.run("app:app", host="0.0.0.0", port=5000, reload=True)
