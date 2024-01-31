import { RoleType } from './roleType';

export interface IAdminUserInfo {
  id: string;
  userName: string;
  password: string;
  displayName: string;
  phoneNumber: string;
  email: string;
  grade: keyof typeof RoleType;
  isAdmin: boolean;
}
