export interface IAdminNews {
    id: number;
    tempId: string;
    title: string;
    caption: string;
    content: string;
    thumbnailUrl: string;
    isShow: boolean;
    isShowByDate: boolean;
    startDate: Date;
    endDate: Date;
  }
  