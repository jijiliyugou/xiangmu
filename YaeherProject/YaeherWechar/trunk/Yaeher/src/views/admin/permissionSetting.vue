<template>
    <div class="permisssion-setting padding-top">
        <mt-header fixed title="权限设置">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <router-link to="/add-admin" class="right-white">添加</router-link>
            </mt-button> 
        </mt-header>
        <div class="content">
            <ul class="">
                <li>
                    <router-link to="/add-admin" class="user-href">
                        <div class="flex permission-case">
                            <div class="admin-title">
                                李云：客服端
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/add-admin" class="user-href">
                        <div class="flex permission-case">
                            <div class="admin-title">
                                李云：质控端
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/add-admin" class="user-href">
                        <div class="flex permission-case">
                            <div class="admin-title">
                                李云：患者端
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/add-admin" class="user-href">
                        <div class="flex permission-case">
                            <div class="admin-title">
                                李云：医生端
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>    
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.permisssion-setting {
    .permission-case {
        position: relative;
        .admin-title {
            font-size: $font-l;
        }
    }
}

</style>