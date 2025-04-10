from pydantic import BaseModel


class LoginRequest(BaseModel):
    UserId: str
    Password: str


class RegisterRequest(BaseModel):
    UserId: str
    Password: str
    Name: str
