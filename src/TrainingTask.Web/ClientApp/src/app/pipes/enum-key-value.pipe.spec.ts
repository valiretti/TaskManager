import { EnumKeyValuePipe } from './enum-key-value.pipe';

describe('EnumKeyValuePipe', () => {
  it('create an instance', () => {
    const pipe = new EnumKeyValuePipe();
    expect(pipe).toBeTruthy();
  });
});
