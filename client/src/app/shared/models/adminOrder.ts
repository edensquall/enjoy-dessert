import { IAddress } from "./address";
import { IOrderItem } from "./order";
import { OrderStatus } from "./orderStatus";

export interface IAdminOrder {
    id: number;
    buyerUserName: string;
    orderDate: Date;
    shipToAddress: IAddress;
    deliveryMethod: string;
    shippingPrice: number;
    orderItems: IOrderItem[];
    subtotal: number;
    total: number;
    status: keyof typeof OrderStatus;
}
