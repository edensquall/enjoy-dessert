export interface IAdminProduct {
    id: number;
    name: string;
    description: string;
    price: number;
    isBestseller: boolean;
    isShow: boolean;
    isShowByDate: boolean;
    startDate: Date;
    endDate: Date;
    productTypeId: number;
    imageUrlsIndex: number[];
    imageUrls: string[];
  }