// Book Interfaces - Backend DTO'larına uygun
export interface Book {
  id?: number;
  title: string;
  description?: string;
  countofPage: number;
  authorId: number;
  categoryId: number;
  recordDate?: Date;
}

export interface BookCreateDto {
  title: string;
  description?: string;
  countofPage: number;
  authorId: number;
  categoryId: number;
}

export interface BookUpdateDto {
  id: number;
  title?: string;
  description?: string;
  countofPage?: number;
  authorId?: number;
  categoryId?: number;
}

export interface BookQueryDto extends Book {
  // API'den gelen tam kitap bilgisi
} 