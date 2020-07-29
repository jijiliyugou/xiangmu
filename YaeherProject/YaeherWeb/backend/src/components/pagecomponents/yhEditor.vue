<template>
  <script :id="randomId" name="content" type="text/plain"></script>
</template>
<script>
export default {
  name: "yh-editor",
  props: {
    defaultMsg: {
      type: String,
      default: ""
    },
    index: {
      default: 0
    }
  },
  data() {
    return {
      myconfig:{autoHeightEnabled:false,toolbars: [[
                'undo', 'redo', '|',
                'bold', 'italic', 'underline', 'fontborder', 'strikethrough', 'superscript', 'subscript','formatmatch', 'removeformat'
                        , '|', 'forecolor', 'cleardoc', '|',
                'rowspacingtop', 'rowspacingbottom', 'lineheight','link', '|','justifyleft','justifyright','justifycenter','justifyjustify', '|',
                'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|', 'simpleupload','preview','fullscreen'
            ]],autoHeightEnabled:false},
      // 为了避免麻烦，每个编辑器实例都用不同的 id
      randomId: "editor_" + Math.random() * 10000,
      instance: null,
    };
  },
  created() {
    this.initEditor();
  },
  beforeDestroy() {
    //this.instance.destroy();
    // 组件销毁的时候，要销毁 UEditor 实例
      // this.instance.ready(function() {
        // this.instance.destroy();
      // })
  },
  watch: {
    defaultMsg(val, oldVal) {
      //this.instance = UE.getEditor(this.randomId, this.myconfig); // 初始化UE
      if (val !== null) {
        this.instance.ready(function() {
          this.setContent(val); // 确保UE加载完成后，放入内容。
        });
      }
    }
  },
  methods: {
    getUEContent() {
      return this.instance.getContent();
    },
    initEditor() {
      let self = this;
      // Vue 异步执行 DOM 更新，这样一来代码执行到这里的时候可能 template 里面的 script 标签还没真正创建
      // 所以，我们只能在 nextTick 里面初始化 UEditor
      this.$nextTick(() => {
        this.instance = UE.getEditor(this.randomId, this.myconfig);
        // 绑定事件，当 UEditor 初始化完成后，将编辑器实例通过自定义的 ready 事件交出去
        this.instance.addListener("ready", () => {
          /*window.UE.Editor.prototype._bkGetActionUrl =
            window.UE.Editor.prototype.getActionUrl;
          window.UE.Editor.prototype.getActionUrl = function(action) {
            if (action == "uploadimage") {
              return "http://192.168.2.3:8001/asp/controller.asp?action=uploadimage&userid="+localStorage.getItem('userId');*/
              /*return "http://upload.integraltel.com/asp/controller.asp?action=uploadimage&userid="+localStorage.getItem('userId');
              return "http://upload.yaeherhealth.com/asp/controller.asp?action=uploadimage&userid="+localStorage.getItem('userId');*/

            /*} else if (action == "uploadvideo") {
              return "http://192.168.2.3:8001/asp/controller.asp?action=uploadimage&userid="+localStorage.getItem('userId');
            } else if (action == "uploadfile") {
              return "http://192.168.2.3:8001/asp/controller.asp?action=uploadimage&userid="+localStorage.getItem('userId');
            } else if (action == "catchimage") {
              return "http://192.168.2.3:8001/asp/controller.asp?action=uploadimage&userid="+localStorage.getItem('userId');
            } else if (action == "uploadscrawl") {
              return "http://192.168.2.3:8001/asp/controller.asp?action=uploadimage&userid="+localStorage.getItem('userId');
            } else if (action == "listimage" || action == "listfile") {
              return "http://192.168.2.3:8001/asp/controller.asp?action=uploadimage&userid="+localStorage.getItem('userId');
            } else if (action == "config") {
              return this._bkGetActionUrl.call(this, action);
            }
          };*/
          this.$emit("ready", this.instance, this.index);
          this.instance.setContent(this.defaultMsg); // 确保UE加载完成后，放入内容。
        });
      });
    }
  }
};
</script>
