export interface Card {
  id: string;
  name: string;
  description: string;
  priority: number;
  dueDate?: string;
  createdAt: string;
  color: string;
  tags: string[];
}
