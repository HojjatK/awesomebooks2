export interface IRetryConfig {
    statusCodes: number[];
    maxRetryAttempts: number;
    delay: number; // in seconds. if 5 seconds delay, then delay = 5
}
export interface IServerMeta {
    failure: string[];
    errors: any;
}

export interface UserSession {
    token: string;
    username: string;
}

export interface IPagination {
    offset: number;
    limit: number;
    previous: string;
    next: string;
    total_records: number;
}

export interface IResponse<T> {
    meta: IServerMeta;
    data: T;
    pagination: IPagination;
}

export interface CategoryGroupModel {
    id: number;
    name: string;
    description: string;
}

export interface CategoryModel {
    id: number;
    name: string;
    category_group_id: number;
    category_group_name: string;
    description: string;
}

export interface BookModel {
    id: number;
    name: string;
    publish_year: number;
    authors: string;
    rating: number;
    image_uri: string;
    amazon_uri: string;
    content_type: string;
    content_uri: string;
    reflection: string;
    category_id: number;
    category_name: string;
}