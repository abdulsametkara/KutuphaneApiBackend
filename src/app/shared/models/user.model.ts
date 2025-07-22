// User Interfaces - Backend DTO'larına uygun
export interface User {
  id?: number;
  name: string;
  surname: string;
  username: string;
  email?: string;
  recordDate?: Date;
}

export interface UserCreateDto {
  name: string;
  surname: string;
  username: string;
  email?: string;
  password: string;
}

export interface UserLoginDto {
  username?: string;
  email?: string;
  password: string;
}

export interface LoginResponse {
  isSuccess: boolean;
  message: string;
  data: string; // JWT token
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  message: string;
  data?: T;
}

export interface JwtPayload {
  name: string;
  sid: string; // user ID
  email: string;
  exp: number; // expiration timestamp
} 