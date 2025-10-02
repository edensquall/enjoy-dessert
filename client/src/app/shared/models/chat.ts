export interface IAsk {
  question: string;
}

export interface IAnswer {
  answer: string;
}

export interface IChat {
  speaker: 'user' | 'ai'; 
  content: string;
  timestamp: Date;
  isHtml?: boolean;
}