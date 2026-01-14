import { Card } from "./card.model";

export interface Column {
  id: string;
  name: string;
  cards: Card[];
}
