import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatOptionBComponent } from './chat-option-b.component';

describe('ChatOptionBComponent', () => {
  let component: ChatOptionBComponent;
  let fixture: ComponentFixture<ChatOptionBComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChatOptionBComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChatOptionBComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
