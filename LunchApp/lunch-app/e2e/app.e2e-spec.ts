import { LunchAppPage } from './app.po';

describe('lunch-app App', function() {
  let page: LunchAppPage;

  beforeEach(() => {
    page = new LunchAppPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
