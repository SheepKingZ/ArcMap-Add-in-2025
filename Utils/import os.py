import os
import re

def main():
    # 获取用户输入的目录路径
    dir_path = input("请输入目录路径: ").strip()
    
    # 检查路径是否存在
    if not os.path.isdir(dir_path):
        print("错误：输入的路径不存在或不是目录！")
        return
    
    print(f"正在处理目录: {dir_path}")
    
    # 遍历目录中的项目
    for item in os.listdir(dir_path):
        item_path = os.path.join(dir_path, item)
        
        # 只处理子目录，跳过文件
        if os.path.isdir(item_path):
            # 提取目录名中的数字部分
            digits = ''.join(re.findall(r'\d+', item))
            
            if digits:
                # 构建新目录名
                new_dirname = digits
                new_path = os.path.join(dir_path, new_dirname)
                
                # 处理目录名冲突
                counter = 1
                while os.path.exists(new_path):
                    new_dirname = f"{digits}_{counter}"
                    new_path = os.path.join(dir_path, new_dirname)
                    counter += 1
                
                # 重命名目录
                try:
                    os.rename(item_path, new_path)
                    print(f"目录重命名成功: '{item}' -> '{new_dirname}'")
                except OSError as e:
                    print(f"目录重命名失败: '{item}' - {str(e)}")
            else:
                print(f"跳过目录 '{item}' (未包含数字)")
        else:
            print(f"跳过文件: '{item}'")

if __name__ == "__main__":
    main()