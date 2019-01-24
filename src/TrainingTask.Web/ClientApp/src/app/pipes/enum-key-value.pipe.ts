import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'enumKeyValue'
})
export class EnumKeyValuePipe implements PipeTransform {

  transform(value: any, args?: any): any {
    const keys = [];
    for (const enumMember in value) {
      if (!isNaN(parseInt(enumMember, 10))) {
        keys.push({key: Number(enumMember), value: value[enumMember]});
      }
    }
    return keys;
  }

}
