export class MenuItem {
    constructor(menuId: number, menuSectionId: number) {
        this.menuId = menuId;
        this.menuSectionId = menuSectionId;
        this.menuItemId = 0;
        this.number = 1;
        this.name = '';
    }

    number: number;
    name: string;
    menuItemId: number;
    menuSectionId: number;
    menuId: number;
}
